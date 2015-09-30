public interface ISub
{
    void Subcribe(string type, string msg); 
    //function that creates a subcription that the object that inherits
    //froms this iterface is looking for 
    void Recive(string msg);
    //recives an event from the observer that the subcribers has subcribed to when
    //the event has been published from some where elese in the program
}

public interface IPub
{
    void Publish(string msg);
    //The class that inherits from this interface will use this function to
    //publish the event trigger that another object/class is subcribed to.
}
