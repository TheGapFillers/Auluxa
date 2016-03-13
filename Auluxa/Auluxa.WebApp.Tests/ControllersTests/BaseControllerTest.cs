using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Auluxa.WebApp.Tests
{
	public abstract class BaseControllerTest<TModel, TController>
		where TController : ApiController, IDisposable
	{
		public const string REQUEST_URI = "http://localhost:57776/api/models";

		public TestDbContext Context { get; set; }
		public TController Controller { get; set; }

		public Func<TModel, TModel> ContextAdd { get; set; }
		public Func<string, Task<IHttpActionResult>> ControllerGet { get; set; }
		public Func<TModel, Task<IHttpActionResult>> ControllerPost { get; set; }
		public Func<TModel, Task<IHttpActionResult>> ControllerPatch { get; set; }
		public Func<int, Task<IHttpActionResult>> ControllerDelete { get; set; }

		public BaseControllerTest() { }

		public virtual void SetUp()
		{
			Context = new TestDbContext();
		}

		public virtual void TearDown()
		{
			if (Controller != null) { Controller.Dispose(); }
			if (Context != null) { Context.Dispose(); }
		}

		public async Task ModelController_GetTest()
		{
			TModel m = BuildTestModel();
			ContextAdd(m);

			var result = await ControllerGet(null) as OkNegotiatedContentResult<List<TModel>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Content.Count);
			AssertModelsAreEqual(result.Content[0], m);
		}

		public async Task ModelController_GetIdTest(string ids, int expectedCount)
		{
			TModel m1 = BuildTestModel(1);
			TModel m2 = BuildTestModel(2);
			ContextAdd(m1);
			ContextAdd(m2);

			var result = await ControllerGet(ids) as OkNegotiatedContentResult<List<TModel>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedCount, result.Content.Count);
		}

		public void ModelController_GetByIdTest_InvalidFormatMustThrow()
		{
			TModel m = BuildTestModel();
			ContextAdd(m);

			//var ex = Assert.ThrowsAsync<FormatException>(async () => await Controller.Get("haha")); //NIY
			Assert.That(async () => await ControllerGet("haha"), Throws.TypeOf<FormatException>());
		}

		public async Task ModelController_PostTest()
		{
			TModel m = BuildTestModel();

			var result = await ControllerPost(m) as CreatedNegotiatedContentResult<TModel>;

			Assert.IsNotNull(result);
			AssertModelsAreEqual(result.Content, m);
		}

		public async Task ModelController_PatchTest(TModel modifiedModel)
		{
			TModel m = BuildTestModel();
			ContextAdd(m);

			var resultPatch = await ControllerPatch(modifiedModel) as CreatedNegotiatedContentResult<TModel>;

			// Patch must return modified model
			Assert.IsNotNull(resultPatch);
			AssertModelsAreEqual(resultPatch.Content, modifiedModel);

			// Get models must return modified model
			var resultGet = await ControllerGet(null) as OkNegotiatedContentResult<List<TModel>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(1, resultGet.Content.Count);
			AssertModelsAreEqual(resultGet.Content[0], modifiedModel);
		}

		public async Task ModelController_DeleteTest(Func<TModel, int> GetModelIdentity)
		{
			TModel m = BuildTestModel();
			ContextAdd(m);

			int id = GetModelIdentity(m);
			// Delete the model, send command and check result
			var resultDelete = await ControllerDelete(id) as OkNegotiatedContentResult<TModel>;

			Assert.IsNotNull(resultDelete);
			AssertModelsAreEqual(resultDelete.Content, m);

			// Get all models must return empty set
			var resultGet = await ControllerGet(null) as OkNegotiatedContentResult<List<TModel>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(0, resultGet.Content.Count);
		}

		protected abstract TModel BuildTestModel(int id = 1);
		protected abstract void AssertModelsAreEqual(TModel expected, TModel actual);
	}
}
