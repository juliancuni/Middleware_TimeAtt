# Middleware for TimeAttendance 
## ArtServis ad hoc project writen on C#
No GUI just a Windows Service.

1. TCP Server listening and interpreting Commands from RestAPI.
2. ZKTeco Client based on ZKTeko SDK Ver 6.3.1.37. Sending Attendances to RestApi and accepting Commands comming from TCP Server
3. RestAPI Client to register new TimeAttendances, register and delete users to ZKTeco Device on TCP Commands comming from API to this Middleware. 

This code is a real mess. You're warned!



[![License]MIT](/LICENSE)  Julian Çuni 2019
