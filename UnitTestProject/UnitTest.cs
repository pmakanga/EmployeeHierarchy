using EmployeeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using static EmployeeLibrary.Employee;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ValidateSalaries()
        {
            //Throw an exception when employee salary is not valid
            Assert.ThrowsException<Exception>(() => new Employee(

                "Employee4,Employee2,500" + 
                "\n" +
                "Employee3,Employee1,800" + 
                "\n" +
                "Employee1,,1000" +
                "\n" +
                "Employee5,Employee1,5yu00" + //Error - employee 5 has an invalid salary
                "\n" + 
                "Employee2,Employee1,500"

             ));
        }

        [TestMethod]
        public void ReportstoMoreThanOneManager()
        {
            // Throw an exception when as employee reports to more than one manager
            Assert.ThrowsException<Exception>(() => new Employee(
                "Employee4,Employee2,500" +
                "\n" +
                "Employee4,Employee3,500" + // error Employee 4 Reports to more than one manager
                "\n" +
                "Employee3,Employee1,500" +
                "\n" +
                "Employee1,,1000" +
                "\n" +
                "Employee5,Employee1,500" +
                "\n" +
                "Employee2,Employee1,800" +
                "\n"+
                "Employee6,Employee2,500"
            ));

        }


        [TestMethod]
        public void MoreThanOneCEO()
        {
            // Throw an exception if we have more than once CEO
            Assert.ThrowsException<Exception>(() => new Employee
            (
                "Employee4,Employee2,500" +
                "\n" +
                "Employee3,Employee1,500" +
                "\n" +
                "Employee1,,1000" +
                "\n" +
                "Employee7,,1000" + // error employee 7 is an additional CEO, we only need one CEO
                "\n" +
                "Employee5,Employee1,500" +
                "\n" +
                "Employee2,Employee1,800" +
                "\n" +
                "Employee6,Employee2,500"
            ));
        }

        [TestMethod]
        public void CircularReference()
        {
            // Throw an exception when we have circular reference
            Assert.ThrowsException<Exception>(() => new Employee
            (
                 "Employee4,Employee2,500" +
                "\n" +
                "Employee3,Employee1,500" +
                "\n" +
                "Employee1,,1000" +
                "\n" +
                "Employee5,Employee1,500" +
                "\n" +
                "Employee2,Employee1,800" +
                "\n" +
                "Employee6,Employee2,500" +
                "\n" +
                "Employee4,Employee1,500" //error Employee 4 is already reporting to employee 2
            ));

        }

        [TestMethod]
        public void ManagerNotListedAsEmployee()
        {
            // Throw an exception if a manager is not listed in the employee column
            Assert.ThrowsException<Exception>(() => new Employee
            (
                "Employee4,Employee2,500" + // error Employee2 is not listed as a manager
                "\n" +
                "Employee3,Employee1,500" +
                "\n" +
                "Employee1,,1000" +
                "\n" +
                "Employee5,Employee1,500" +
                "\n" +
                "Employee6,Employee2,500"

              ));

        }

        [TestMethod]
        public void ManagerSalaryBudget()
        {
            Employee employee = new Employee(

                "Employee4,Employee2,500" +
                "\n" +
                "Employee3,Employee1,500" +
                "\n" +
                "Employee1,,1000" +
                "\n" +
                "Employee5,Employee1,500" +
                "\n" +
                "Employee2,Employee1,800" +
                "\n" +
                "Employee6,Employee2,500" 

                );
            // Input = Employee1, Total budget for the manager is 3800 i.e Employee1 + Employee2 + Employee3 + Employee4 + Employee5 + Employee6
            Assert.AreEqual(3800, employee.managerSalaryBudget("Employee1"));

        }
    }
}
        