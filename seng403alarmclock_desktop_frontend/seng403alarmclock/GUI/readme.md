=================================================
               GUI API DESCRIPTION
=================================================

For controlling the GUI, use GuiController, it is a singleton class with these public methods

      class GuiController {
        GetController()           - To access the singleton
        SetTime(DateTime time)    - To set the time on the GUI
        AddAlarm(Alarm alarm)     - To add a new alarm
        TriggerAlarm(Alarm alarm) - To set the alarm off
        RemoveAlarm(Alarm alarm)  - To remove the alarm from the gui  
      }
      
For responsing to the gui, you should implement GuiListener, and register it with GuiEventCaller like this

    GuiEventCaller.getCaller().AddListener(new GuiEventListener() {});
    
The GuiListener interface has the following events:

    interface GuiListener {
       void AlarmCanceled(Alarm alarm);           - When the user attempts to cancel an alarm
       void AlarmDismissed(Alarm alarm);          - When the user attempts to dismiss an alarm
       void AlarmRequested(int hour, int minute); - When the user attempts to set a new alarm
    }
   
     


