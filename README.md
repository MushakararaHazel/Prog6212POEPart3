# Prog6212POEPart3
Youtube video - https://youtu.be/PaslrkO4mXI
Powerpink link - https://advtechonline-my.sharepoint.com/:p:/g/personal/st10443516_imconnect_edu_za/IQAX7R6ttjSIToYb8zRJkdlKAWoqTkNm7wN8ElKv00M-XRA?e=xa6Fi6

Contract Monthly Claim System (CMCS) — Project Report


The Contract Monthly Claim System (CMCS) is a web-based application developed to streamline the submission, verification, approval, and payment of monthly claims for contract lecturers. The system provides a role-based workflow involving Lecturers, Programme Coordinators, Academic Managers, and HR administrators. CMCS ensures accuracy, transparency, and efficiency by introducing automated calculations, database-driven user data, document uploads, and controlled approval processes. This report outlines major enhancements implemented from Part 2 to Part 3, focusing on improved usability, automation, and backend structure.The CMCS application enables contract lecturers to submit their monthly work hours and supporting documents. Coordinators then review and verify the claims, managers approve them, and HR users generate reports and manage user profiles. Role-based access control ensures each user can access only the functions relevant to their position. The system is built using ASP.NET Core MVC, Entity Framework Core, SQL Server, QuestPDF, and Bootstrap for UI styling.

 Enhancements and Updates from Part 2 to Part 3
In Part 3, lecturer details such as First Name, Last Name, Email, and Hourly Rate are now automatically fetched from the database using Entity Framework. This eliminates manual input errors and ensures consistency across all claims. When a lecturer logs in, the system retrieves their stored profile information, allowing them to focus solely on entering hours worked and attaching supporting documents.The system now automatically computes the Total Claim Amount using the formula: Total = Hours Worked × Hourly Rate.This enhances accuracy and ensures lecturers cannot manipulate or accidentally alter the hourly rate.New validation rules were added to ensure data integrity. Claims cannot exceed 180 hours per month, preventing unrealistic submissions. Validation messages provide real-time feedback to lecturers during submission. Part 3 introduced refined role-based restrictions through [Authorize(Roles = "...")] attributes.

Lecturers can submit and track claims.

Coordinators can verify claims.

Managers can approve and mark claims as paid.

HR users manage all users and generate system reports.

Attempts to access unauthorized pages redirect the user to an "Access Denied" view.


HR can now generate cleaner, formatted PDF reports using QuestPDF. A filter was added to allow HR to generate lecturer-only reports, ensuring only relevant users are displayed. The PDF layout includes columns for ID, Username, Full Name, Email, Hourly Rate, and Role.User management now supports:

Creating new accounts

Editing user details

Resetting passwords

Viewing all registered users

Downloading reports

The UI has been streamlined using Bootstrap tables and action badges.

 System Features
 Lecturer Features

Submit monthly claims

Upload supporting documents (PDF, JPG, PNG)

Automatic total calculation

Track claim progress

View history of submissions

.2 Coordinator Features

Access pending lecturer claims

Verify or reject claims

Add verification notes

.3 Manager Features

Review verified claims

Approve and mark claims as paid

Monitor claim flow

.4 HR Features

Manage all system users

Add, edit, or delete user accounts

Generate PDF system reports

Access full system analytic

Part 3 makes CMCS significantly more efficient, secure, and user-friendly by automating calculations, enforcing role-based restrictions, improving report generation, and refining the entire claim approval process. The system is now more reliable and ready for real-world use.
