using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Auluxa.WebApp.Tests
{
	public abstract class BaseControllerTest<Model, TestedController>
		where TestedController : ApiController, IDisposable, new()
	{
		public TestDbContext Context { get; set; }
		public TestedController Controller { get; set; }

		public virtual void SetUp()
		{
			Context = new TestDbContext();
		}

		public virtual void TearDown()
		{
			if (Controller != null) { Controller.Dispose(); }
			if (Context != null) { Context.Dispose(); }
		}

		public async Task ModelController_GetTest(Func<Model, Model> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet)
		{
			Model m = BuildTestModel();
			AddToContext(m);

			var result = await ControllerGet(null) as OkNegotiatedContentResult<List<Model>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Content.Count);
			AssertModelsAreEqual(result.Content[0], m);
		}

		public async Task ModelController_GetIdTest(string ids, int expectedCount, Func<Model, Model> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet)
		{
			Model m1 = BuildTestModel(1);
			Model m2 = BuildTestModel(2);
			AddToContext(m1);
			AddToContext(m2);

			var result = await ControllerGet(ids) as OkNegotiatedContentResult<List<Model>>;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedCount, result.Content.Count);
		}

		public void ModelController_GetByIdTest_InvalidFormatMustThrow(Func<Model, Model> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet)
		{
			Model m = BuildTestModel();
			AddToContext(m);

			//var ex = Assert.ThrowsAsync<FormatException>(async () => await Controller.Get("haha")); //NIY
			Assert.That(async () => await ControllerGet("haha"), Throws.TypeOf<FormatException>());
		}

		public async Task ModelController_PostTest(Func<Model, Task<IHttpActionResult>> ControllerPost)
		{
			Model m = BuildTestModel();

			var result = await ControllerPost(m) as CreatedNegotiatedContentResult<Model>;

			Assert.IsNotNull(result);
			AssertModelsAreEqual(result.Content, m);
		}

		public async Task ModelController_PatchTest(Model modifiedModel, Func<Model, Model> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet, Func<Model, Task<IHttpActionResult>> ControllerPatch)
		{
			Model m = BuildTestModel();
			AddToContext(m);

			var resultPatch = await ControllerPatch(modifiedModel) as CreatedNegotiatedContentResult<Model>;

			// Patch must return modified model
			Assert.IsNotNull(resultPatch);
			AssertModelsAreEqual(resultPatch.Content, modifiedModel);

			// Get models must return modified model
			var resultGet = await ControllerGet(null) as OkNegotiatedContentResult<List<Model>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(1, resultGet.Content.Count);
			AssertModelsAreEqual(resultGet.Content[0], modifiedModel);
		}

		public async Task ModelController_DeleteTest(Func<Model, int> GetModelIdentity, Func<Model, Model> AddToContext, Func<string, Task<IHttpActionResult>> ControllerGet, Func<int, Task<IHttpActionResult>> ControllerDelete)
		{
			Model m = BuildTestModel();
			AddToContext(m);

			int id = GetModelIdentity(m);
			// Delete the model, send command and check result
			var resultDelete = await ControllerDelete(id) as OkNegotiatedContentResult<Model>;

			Assert.IsNotNull(resultDelete);
			AssertModelsAreEqual(resultDelete.Content, m);

			// Get all models must return empty set
			var resultGet = await ControllerGet(null) as OkNegotiatedContentResult<List<Model>>;

			Assert.IsNotNull(resultGet);
			Assert.AreEqual(0, resultGet.Content.Count);
		}

		protected abstract Model BuildTestModel(int id = 1);
		protected abstract void AssertModelsAreEqual(Model expected, Model actual);
	}
}
