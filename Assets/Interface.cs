public interface ISub
{
    void Subcribe(string type, string msg, CallBacks func); 
    //function that creates a subcription that the object that inherits
    //froms this iterface is looking for 
}

public interface IPub
{
    void Publish(string msg);
    //The class that inherits from this interface will use this function to
    //publish the event trigger that another object/class is subcribed to.
}
