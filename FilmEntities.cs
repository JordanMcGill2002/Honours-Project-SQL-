using System; 

namespace FilmEntities
{
public class Film {

    public Int32 FilmId {get; set;}
    public String Title {get; set;}
    public String Genre {get; set;}
    public String Rating {get; set;}
    public string Age_Rating {get; set;}
    public string Release_Date {get; set;}
    public Int32 ActorID {get; set;}

    }

    public class Actor 
    {

        public Int32 ActorID {get; set;}
        public String Forename {get; set;}
        public String Surname {get; set;}
        public String DOB { get; set; }
        public Int32 FilmId {get; set;}


    }

     public class FilmActor
        {

            public Int32 FilmId {get;set;}
            public Int32 ActorID {get;set;}
            public String Title {get;set;}
            public String FName {get;set;}
            public String SName {get;set;}
            public String Genre {get;set;}
            public String Release {get;set;}
            public String Age {get;set;}
            public String Rating {get;set;}
        }
}