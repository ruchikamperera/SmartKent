using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartKent.Models;

namespace SmartKent.Controllers
{
    public class SmartKentController : Controller
    {
        // initialize the 2 lifts with its states and other default values
      public static  IList<Lift> Lifts = new List<Lift>()
        {
            new Lift(){ Id=1, State="Idle" , Direction= String.Empty , Floor =1 , Person = 0},
            new Lift(){ Id=2, State="Idle" , Direction= String.Empty , Floor =1 , Person = 0},

        };

        int duration = 3; // duration to go between 2 floors
        int pickUpDuration = 4; // duration to pickup/drop off
        // constructor
        public void SmartKent()
        {
            
        }


        [HttpGet]
        [Route("SmartKent/PickAndDrop/{fromFloor},{toFloor}")]
        public string PickAndDrop(int fromFloor, int toFloor)
        {
            // return value
            string result = String.Empty;
            int totalTimeDurationToPickUp = 0;  // total time that takes to reach the floor
            int totalTimeDurationToReachToFloor = 0; // total time to reach the from floor to to floor
            int TimeDurationToGetIn = 4;  // total time that takes the person to get in  to lift
            int TimeDurationToGetOut = 4;  // total time that takes the person to get  out to lift



            // check the 1st lift is busy or not
            if (SmartKentController.Lifts.ElementAt(0).State =="Idle")
            {
                    // 1st lift is free 

                    if(fromFloor> SmartKentController.Lifts.ElementAt(0).Floor)
                    {
                        // lift should go up to pick up the person
                        totalTimeDurationToPickUp = (fromFloor - SmartKentController.Lifts.ElementAt(0).Floor) * this.duration;
                        SmartKentController.Lifts.ElementAt(0).Direction = "Up";// set direction

                        /* total time to pick up the person equals to number of floors that
                         * lift should come from its current floor to the floor that person stays */


                    } else
                    {
                        // lift should go down to pick up the person
                        totalTimeDurationToPickUp = (SmartKentController.Lifts.ElementAt(0).Floor - fromFloor) * this.duration;

                        SmartKentController.Lifts.ElementAt(0).Direction = "Down";// set direction

                        /* total time to pick up the person equals to number of floors that
                        * lift should come from its current floor to the floor that person stays */


                    }
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    // set the 1st lift has started to move and immediatly change state as TO_PICKUP                 
                    this.ChangeLiftStatus(1, 0, "TO_PICKUP");

                }).Start();

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    // after totaltime duration state will be changed to pick up since the life has reached the person
                    this.ChangeLiftStatus(1, totalTimeDurationToPickUp, "PICKUP ");

                }).Start();

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    this.ChangeLiftStatus(1, TimeDurationToGetIn, "TO_DROPOFF "); // after the person get in the lift the state should be "To DropOff"

                }).Start();


                // count the duration to reach the destination 

                if (fromFloor>toFloor)
                {
                    totalTimeDurationToReachToFloor = (fromFloor - toFloor) * duration;
                } else
                {
                    totalTimeDurationToReachToFloor = (toFloor-fromFloor  ) * duration;

                }


                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    // lift has finished moved and DROPOFF  the person and change the state to "DROPOFF "
                    this.ChangeLiftStatus(1, totalTimeDurationToReachToFloor, "DROPOFF");
                }).Start();
                


                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    // person is get out from the lift and after 4 seconds state should be IDLE
                    this.ChangeLiftStatus(1, TimeDurationToGetOut, "IDLE");
                }).Start();

                // set the current floor of the lift
                SmartKentController.Lifts.ElementAt(0).Floor = toFloor;




            }
            else if(SmartKentController.Lifts.ElementAt(1).State == "Idle")
            {
                    // 1st lift is busy and 2nd lift is free 

                    if (fromFloor > SmartKentController.Lifts.ElementAt(1).Floor)
                    {
                        // lift should go up to pick up the person
                        totalTimeDurationToPickUp = (fromFloor - SmartKentController.Lifts.ElementAt(1).Floor) * this.duration;

                        /* total time to pick up the person equals to number of floors that
                         * lift should come from its current floor to the floor that person stays */

                        SmartKentController.Lifts.ElementAt(1).Direction = "Up";// set direction
                     }
                    else
                    {
                      // lift should go down to pick up the person
                      totalTimeDurationToPickUp = (SmartKentController.Lifts.ElementAt(1).Floor - fromFloor) * this.duration;

                        /* total time to pick up the person equals to number of floors that
                       * lift should come from its current floor to the floor that person stays */

                        SmartKentController.Lifts.ElementAt(1).Direction = "Down";// set direction

                   
                    }


                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        // set the 2nd lift has started to move and immediatly change state as TO_PICKUP                 
                        this.ChangeLiftStatus(2, 0, "TO_PICKUP");
                    }).Start();
               


                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        // after totaltime duration, state will be changed to pick up since the life has reached the person
                        this.ChangeLiftStatus(2, totalTimeDurationToPickUp, "PICKUP ");
                    }).Start();
               


                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        this.ChangeLiftStatus(2, TimeDurationToGetIn, "TO_DROPOFF "); // after the person get in the lift the state should be "To DropOff"
                    }).Start();
               

                    // count the duration to reach the destination 

                    if (fromFloor > toFloor)
                    {
                        totalTimeDurationToReachToFloor = (fromFloor - toFloor) * duration;
                    }
                    else
                    {
                        totalTimeDurationToReachToFloor = (toFloor - fromFloor) * duration;

                    }


                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    // lift has finished moved and DROPOFF  the person and change the state to "DROPOFF "
                    this.ChangeLiftStatus(2, totalTimeDurationToReachToFloor, "DROPOFF");
                }).Start();
                

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    // person is get out from the lift and after 4 seconds state should be IDLE
                    this.ChangeLiftStatus(2, TimeDurationToGetOut, "IDLE");
                }).Start();

                // set the current floor of the lift
                SmartKentController.Lifts.ElementAt(1).Floor = toFloor;
                

            } else
            {
                result = "Both lifts are busy";
            }

            result = "ETA - " + totalTimeDurationToPickUp.ToString();

            return result;
        }


        public void ChangeLiftStatus(int liftId , int duration , string status)
        {
            int durationInMiliSeconds = duration * 1000; // task.delay method accept miliseconds 
            if(liftId ==1)
            {
                Task.Delay(durationInMiliSeconds).ContinueWith(t=>
                    // this will run after durationInMiliSeconds
                    SmartKentController.Lifts.ElementAt(0).State = status  // 1st lift's states will be changed according to the given status
                );
            } else if(liftId ==2)
            {
                Task.Delay(durationInMiliSeconds).ContinueWith(t =>

                 SmartKentController.Lifts.ElementAt(1).State = status  // // 2nd lift's states will be changed according to the given status
            );
            } else
            {
                Console.WriteLine("Invalid Lift Id");
            }
        }

      
    }
}