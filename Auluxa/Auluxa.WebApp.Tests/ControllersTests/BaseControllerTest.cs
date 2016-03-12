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
		public TestDbContext Context { get; set; }
		public TController Controller { get; set; }

		public virtual void SetUp()
		{
			Context = new TestDbContext();
		}

		public virtual void TearDown()
		{
			if (Controller != null) { Controller.Dispose(); }
			if (Context != null) { Context.Dispose(); }
		}

		public async Task ModelController_GetTest(Func<TModel, TModel> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet)
		{
			TModel m = BuildTestModel();
			AddToContext(m);

			var result = await ControllerGet(null) as OkNegotiatedContentResult<List<TModel>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Content.Count);
			AssertModelsAreEqual(result.Content[0], m);
		}

		public async Task ModelController_GetIdTest(string ids, int expectedCount, Func<TModel, TModel> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet)
		{
			TModel m1 = BuildTestModel(1);
			TModel m2 = BuildTestModel(2);
			AddToContext(m1);
			AddToContext(m2);

			var result = await ControllerGet(ids) as OkNegotiatedContentResult<List<TModel>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedCount, result.Content.Count);
		}

		public void ModelController_GetByIdTest_InvalidFormatMustThrow(Func<TModel, TModel> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet)
		{
			TModel m = BuildTestModel();
			AddToContext(m);

			//var ex = Assert.ThrowsAsync<FormatException>(async () => await Controller.Get("haha")); //NIY
			Assert.That(async () => await ControllerGet("haha"), Throws.TypeOf<FormatException>());
		}

		public async Task ModelController_PostTest(Func<TModel, Task<IHttpActionResult>> ControllerPost)
		{
			TModel m = BuildTestModel();

			var result = await ControllerPost(m) as CreatedNegotiatedContentResult<TModel>;

			Assert.IsNotNull(result);
			AssertModelsAreEqual(result.Content, m);
		}

		public async Task ModelController_PatchTest(TModel modifiedModel, Func<TModel, TModel> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet, Func<TModel, Task<IHttpActionResult>> ControllerPatch)
		{
			TModel m = BuildTestModel();
			AddToContext(m);

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

		public async Task ModelController_DeleteTest(Func<TModel, int> GetModelIdentity, Func<TModel, TModel> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet, Func<int, Task<IHttpActionResult>> ControllerDelete)
		{
			TModel m = BuildTestModel();
			AddToContext(m);

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
