using  System;
namespace SehatNotebook.Entities.DBSet{
  public abstract class BaseEntity{
         public Guid Id { get; set; }=new Guid{};
         public int Status { get; set; }=1;
         public DateTime AddedDate { get; set; }=DateTime.Now;
         public DateTime UpdateDate { get; set; }

  }
}