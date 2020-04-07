# TakeHomeTest
# Take home sql assignment

#solution : 1
---------------------

with duplicates

As
(select *,ROW_NUMBER()  over
(PARTITION by empID, DEPtid Order by ID,Name) as Duplicate From EMPLOYEE)

delete From duplicates

Where Duplicates >1;

#solution  : 2
----------------------
command will execute successful but will not drop table

delete from employee where deptid not in (select deptid from department)

#Take home Ruby Assignment
---------------
i have uploaded chartrequest.cs file

#Take home React Assignment
-----------------
i have uploaded React project with redux
1.please make sure npm install before start

