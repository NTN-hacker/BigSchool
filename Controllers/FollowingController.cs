using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BigSchool.Models;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers
{
    public class FollowingController : ApiController
    {
        BigSchoolContext data = new BigSchoolContext();
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
           // try {
                //user login là người theo dõi và FolloweeId chính là Id của người được theo dõi (theo LecturerId)
                var userID = User.Identity.GetUserId();
                if (userID == null)
                    return BadRequest("Hãy đăng nhập để theo dõi một giảng viên");
                if (userID == follow.ForlloweeId)
                    return BadRequest("Không tự theo dõi chính mình!");
                //xử lý button theo dõi
               
                //kiem tra xem doi tượng có đang theo dõi không?
                Following find = data.Followings.FirstOrDefault(p => p.FollowerId == userID && p.ForlloweeId == follow.ForlloweeId);
                if (find != null)
                {
                //return BadRequest("Bạn đang theo dõi giảng viên này!");
                data.Followings.Remove(data.Followings.SingleOrDefault(p => p.FollowerId == userID && p.ForlloweeId == follow.ForlloweeId));
                data.SaveChanges();
                return Ok("cancel");
            }
                //lưu dữ liệu theo dõi
                follow.FollowerId = userID;
                //follow.ForlloweeId = "d85c1be6-9bb5-4ba7-8edd-344dd32be863";
                data.Followings.Add(follow);
                data.SaveChanges();
           // }
            //catch(Exception ex) {
               
            //}
           
            return Ok();
        }
    }
}
