using System.Linq;
using Documentation.Messages;
using Microsoft.AspNetCore.Mvc;
using TestApi.Entity;
using TestApi.Interfaces;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        IMethod _SMethod;
        IDocument _SDocument;
        public UserController(IMethod _SMethod, IDocument _SDocument)
        {
            this._SMethod = _SMethod;
            this._SDocument = _SDocument;
        }

        /// <summary>
        /// Test Get Method
        /// </summary>
        /// <returns></returns>
        [HttpGet("testmethod1")]
        public IActionResult Get_Test_Method_1()
        {
            BaseResult<User> baseResult = new BaseResult<User>();
            baseResult.data.id = 1;
            baseResult.message = Messages.getUser;
            return Json(baseResult);
        }

        /// <summary>
        /// Test post method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("testpostmethod")]
        public IActionResult Test_Post_Method(int id)
        {
            BaseResult<User> baseResult = new BaseResult<User>();
            baseResult.data.id = id;
            baseResult.message = Messages.insertUser;
            return Json(baseResult);
        }

        /// <summary>
        /// Test Put Method
        /// </summary>
        /// <returns></returns>
        [HttpPut("testputmethod/{id:int}")]
        public IActionResult Test_Put_Method(int id)
        {
            BaseResult<User> baseResult = new BaseResult<User>();
            baseResult.data.id = id;
            baseResult.message = Messages.updateUser;
            return Json(baseResult);
        }

        /// <summary>
        /// Test delete method
        /// </summary>
        /// <returns></returns>
        [HttpDelete("testdeletemethod/{id:int}")]
        public IActionResult Test_Delete_Method(int id)
        {
            BaseResult<User> baseResult = new BaseResult<User>();
            baseResult.data.id = id;
            baseResult.message = Messages.deleteUser;
            return Json(baseResult);
        }

        /// <summary>
        /// Returns Necessary Information For The Document
        /// </summary>
        /// <returns></returns>
        [HttpGet("documentinfo")]
        public IActionResult Get_Document_Info()
        {
            BaseResult<DocumentKeyValue> baseResult = new BaseResult<DocumentKeyValue>();
            var result = _SDocument.Create_Document_Info();
            baseResult.data.key = result.FirstOrDefault().Key;
            foreach (var value in result.FirstOrDefault())
            {
                baseResult.data.value.Add(value);
            }
            return Json(baseResult);
        }
    }
}

