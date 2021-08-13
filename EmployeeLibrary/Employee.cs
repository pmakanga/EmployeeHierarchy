using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmployeeLibrary
{
    public class Employee
    {
        ArrayList employeeList;
        public Employee(string csv)
        {
            try
            {
                employeeList = filterCSVToArray(csv);
                ValidateEmployeeData(employeeList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ArrayList filterCSVToArray(string csv)
        {
            ArrayList cleanedData = new ArrayList();
            if (string.IsNullOrEmpty(csv) || !(csv is string))
            {
                throw new Exception("csv cannot be null");
            }

            string[] datarows = csv.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
            );

            foreach (string row in datarows)
            {
                string[] data = row.Split(',');

                ArrayList filteredData = new ArrayList();

                foreach (string cell in data)
                {
                    filteredData.Add(cell);
                }
                if (filteredData.Count != 3)
                {
                    throw new Exception("CSV file must have 3 values in each row");
                }
                cleanedData.Add(filteredData);
            }

            return cleanedData;
        }

       
        //call employee validation methods
        public void ValidateEmployeeData(ArrayList employees)
        {

            try
            {
                ValidateSalaries(employees);
                ReportstoMoreThanOneManager(employees); // check if employee reports to more than one manager
                MoreThanOneCEO(employees);
                CircularReference(employees);
                ManagerNotListedAsEmployee(employees);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        //Check employees salary if they are valid integer numbers
        public void ValidateSalaries(ArrayList employeeList)
        {
            foreach (ArrayList employee in employeeList)
            {
                string employeeSalary = Convert.ToString(employee[2]);
                int number;
                if (!(Int32.TryParse(employeeSalary, out number)))
                {
                    throw new Exception("Employees salaries must be a valid integer");
                }
            }
        }

        // Check if employee reports to one manager
        public void ReportstoMoreThanOneManager(ArrayList employees)
        {
            ArrayList savedEmployees = new ArrayList();

            foreach (ArrayList employee in employees)
            {
                string employeename = employee[0] as string;

               
                if (savedEmployees.Contains(employeename.Trim()))
                {
                    throw new Exception("An employee should not report to more than one manager");
                }

                savedEmployees.Add(employeename.Trim());

            }
        }

        // Check if we only have one CEO
        public void MoreThanOneCEO(ArrayList employees)
        {
            ArrayList managers = new ArrayList(); // manager listing
            ArrayList ceos = new ArrayList();

            foreach (ArrayList employee in employees)
            {
                
                string employeename = employee[0] as string;
                string managername = employee[1] as string;

                if (!string.IsNullOrEmpty(managername.Trim()))
                {
                    managers.Add(managername.Trim());
                }
                else
                {
                    ceos.Add(employeename.Trim());
                }

            }

            int managersDiff = employees.Count - managers.Count;
            if (managersDiff != 1)
            {
                throw new Exception("We must have one CEO");
            }
        }

        // check for circular reference
        public void CircularReference(ArrayList employees)
        {
            ArrayList savedEmployees = new ArrayList();
            ArrayList managers = new ArrayList();
            ArrayList ceos = new ArrayList();
            ArrayList juniorEmployees = new ArrayList();

            foreach (ArrayList employee in employees)
            {
                string employeename = employee[0] as string;
                string managername = employee[1] as string;

                savedEmployees.Add(employeename.Trim());


                if (!string.IsNullOrEmpty(managername.Trim()))
                {
                    managers.Add(managername.Trim());
                }
                else
                {
                    ceos.Add(employeename.Trim());
                }

               // add a junior employee
                foreach (string _employee in savedEmployees)
                {
                    if (!managers.Contains(_employee) && !ceos.Contains(_employee))
                    {
                        juniorEmployees.Add(_employee.Trim());
                    }
                }
            }
      
            for (var i = 0; i < employees.Count; i++)
            {
                var employeeData = employees[i] as ArrayList;
                var employeeManager = employeeData[1] as string;
                int index = savedEmployees.IndexOf(employeeManager);

                if (index != -1)
                {
                    var managerData = employees[index] as ArrayList;
                    var topManager = managerData[1] as string;

                    if ((managers.Contains(topManager.Trim()) && !ceos.Contains(topManager.Trim()))
                        || juniorEmployees.Contains(topManager.Trim()))
                    {
                        throw new Exception("Circular reference error");
                    }
                }
            }
        }
        
        // check if a manager is listed as an employee
        public void ManagerNotListedAsEmployee(ArrayList employees)
        {
            ArrayList managers = new ArrayList();
            ArrayList ceos = new ArrayList();
            ArrayList savedEmployees = new ArrayList();

            foreach (ArrayList employee in employees)
            {
                string employeename = employee[0] as string;
                string managername = employee[1] as string;

                savedEmployees.Add(employeename.Trim());

                if (!string.IsNullOrEmpty(managername.Trim()))
                {
                    managers.Add(managername.Trim());
                }
                else
                {
                    ceos.Add(employeename.Trim());
                }
            }

            foreach (string manager in managers)
            {
                if (!savedEmployees.Contains(manager.Trim()))
                {
                    throw new Exception("There is a manager who is not listed as an employee");
                }
            }
        }
        public long managerSalaryBudget(string managerName)
        {
            try
            {
                long totalSalaryBudget = 0;
                
                foreach (ArrayList employee in employeeList)
                {
                    var name = employee[1] as string;
                    var employeeSalary = employee[2] as string;
                    var employeeName = employee[0] as string;
                    if (name.Trim() == managerName.Trim() || employeeName.Trim() == managerName.Trim())
                    {
                        
                        var managerId = employee[0];
                        if(managerId.ToString() == managerName)
                        {
                            var managerSalary = employee[2];
                            totalSalaryBudget += Convert.ToInt32(managerSalary); // include manager's salary on the total budget
                        }
                        totalSalaryBudget += Convert.ToInt64(employeeSalary);
                       
                    }
                }

                return totalSalaryBudget;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

    }

}
