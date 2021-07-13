namespace BigSchool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string LecturerId { get; set; }
        [Required]
        [StringLength(255, ErrorMessage ="Place vui lòng nhập không quá 255 kí tự")]
        public string Place { get; set; }
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public DateTime? Datetime { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public string Name;
        //chèn list category
        public List<Category> listCategory = new List<Category>();
        public virtual Category Category { get; set; }
        public object LectureName { get; internal set; }
    }
}
