using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;


public class ScoreUpdate : MonoBehaviour
{

    private string URI = "mongodb://127.0.0.1:27017/";
    private string DATABASENAME = "test";
    private MongoClient client;
    private IMongoDatabase database;

    public string useremail;
    public string userphonenumber;
    public float userscore;

    //private Regex emailregex = new Regex(@"^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");

    // Start is called before the first frame update
    void Start()
    {
      UpdateScore(useremail, userphonenumber, userscore);
      Debug.Log("All Fine");
    }

    void UpdateScore(string useremail, string userphonenumber, float userscore){

      if(Regex.IsMatch(useremail, @"^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")){
        if(userphonenumber.Length == 10){
          client = new MongoClient(URI);
          database = client.GetDatabase(DATABASENAME);
          var collection = database.GetCollection<BsonDocument>("Users");

          var filter = Builders<BsonDocument>.Filter.Eq("PhoneNumber", userphonenumber);
          var update = Builders<BsonDocument>.Update.Set("Score", userscore);

          var results = collection.Find(filter).FirstOrDefault();
          if(results != null){
              collection.UpdateOne(filter,update);
          }
          else{
              var newuser = new BsonDocument {
                  {"Email" , useremail},
                  {"PhoneNumber" , userphonenumber},
                  {"Score" , userscore}
              };
              collection.InsertOne(newuser);
          }
        }else{
          Debug.Log("Phone Number has Wrong Lenght");
        }
      }else{
        Debug.Log("Email has wrong format");
      }

    }
}
