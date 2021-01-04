using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop.ViewModels
{
    public class AboutUsViewModel
    {
        public List<Member> Members { get; set; }

        /// <summary>
        /// Hàm khởi tạo mặc định đối tượng
        /// </summary>
        public AboutUsViewModel()
        {
            Members = new List<Member>();

            var member1 = new Member
            {
                Name = "Nguyễn Hoàng Khang",
                AvatarImage="../Data/Images/0039.jpg",
                Job = "Student",
                Position = "PM & Design",
                Gmail = "nhk25022016",
                Facebook = "nhkitusk18"
            };
            var member2 = new Member
            {
                Name = "Bùi Huỳnh Trung Tín",
                AvatarImage="../Data/Images/0092.jpg",
                Job = "Student",
                Position = "Tester",
                Gmail = "bhtt190800",
                Facebook = "bhtt190800"
            };
            var member3 = new Member
            {
                Name = "Trương Đại Triều",
                AvatarImage="../Data/Images/0096.jpg",
                Job = "Student",
                Position = "Dev & Designer",
                Gmail = "truongdaitrieu2109",
                Facebook = "truong.daitrieu.5"
            };

            AddMember(member1);
            AddMember(member2);
            AddMember(member3);

        }

        /// <summary>
        /// Hàm thêm thành viên mới vào team
        /// </summary>
        /// <param name="member"></param>
        public void AddMember(Member member)
        {
            Members.Add(member);
            member.Id = Members.Count.ToString();
        }
    }
    public class Member
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AvatarImage { get; set; }
        public string Job { get; set; }
        public string Position { get; set; }
        public string Gmail { get; set; }
        public string Facebook { get; set; }

        public Member()
        {

        }
    }


}
