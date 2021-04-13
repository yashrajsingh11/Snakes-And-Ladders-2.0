using Firebase.Firestore;

[FirestoreData]
public class RoomsDataXXXX
{   
    [FirestoreProperty]
    public string player1Uid { get; set; }
    [FirestoreProperty]
    public string player2Uid { get; set; }

    [FirestoreProperty]
    public string dateTime { get; set; }

    [FirestoreProperty]
    public string roomKey { get; set; }

    [FirestoreProperty]
    public bool player1Move { get; set; }
    [FirestoreProperty]
    public bool player2Move { get; set; }
}