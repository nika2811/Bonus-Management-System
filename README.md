Create an application that allows you to give bonuses to employees and calculate various statistics.

For each employee, it should be possible to give a bonus of 50% to 300% of his salary.

Any specified employee can have a recommending employee (on whose recommendation they were hired).

In case of giving a bonus, if the employee has a recommended recommender, he should also be given a bonus, the amount of which should be 50% of the original bonus.

The bonus distribution chain can extend to max. 3 levels.

e.g. There are three employees in the system:

1. Employee1
2. Employee2
3. Employee3
4. Employee4

If Employee4 is hired on the recommendation of Employee3. Employee3 was hired on the recommendation of Employee2 and Employee2 was hired on the recommendation of Employee1, then in case of giving a bonus of 500 GEL to Employee4, the bonuses will be distributed to the recommending employees as follows:

Employee4 - 500 GEL

Employee3 - 250 GEL

Employee2 - 125 GEL

Employee1 - 0 GEL

Employees must have the following basic fields:

- Name
- last name
- personal number
- Remuneration
- recommender
- Date of commencement of work

There should be unit tests to check the accuracy of counting bonuses.

The application should also be able to:

- Counting the number of bonuses issued on specified dates
- Find the top 10 employees who received the most bonuses
- Finding the top 10 employees with the highest referral bonus