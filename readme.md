# Hospital Project 

## Booking Appointment Feature (Franck Cheuzem)

This feature enable a Patient to book an appointment with a Physician and Vice Versa.


### MVP functionalities

- [x] CRUD completed for the appointments table. Can create, read, update, and delete records in the table
 through views.
 
![image of list of appointments](https://github.com/Journeygault/Timmins_V3/blob/master/imgs/list_appointments.jpg)

![image of appointment details](https://github.com/Journeygault/Timmins_V3/blob/master/imgs/details_appointment.jpg)

![image of appointment creation](https://github.com/Journeygault/Timmins_V3/blob/master/imgs/create_appointment.jpg)

![image of appointment edition](https://github.com/Journeygault/Timmins_V3/blob/master/imgs/edit_appointment.jpg)

![image of appointment deletion](https://github.com/Journeygault/Timmins_V3/blob/master/imgs/delete_appointment.jpg)
 
- [x] validation of  date and time requested for an appointment
- [x] Adding of Admin , Patient and Physician privileges
- [x] Styling of the pages
- [x] form validation
- [x] Assign a user role to users through the user registeration form as well as firstname, lastname and username.

![image of user registration form](https://github.com/Journeygault/Timmins_V3/blob/master/imgs/user_registration.jpg)

- [x] Allow logged in users to view their profile 

![image of appointment deletion](https://github.com/Journeygault/Timmins_V3/blob/master/imgs/user_profile.jpg)


## Extra features
- [x] pagination for the list of appointments.
- [x] seed roles table with "Admin", "Patient", and "Physician" roles
- [x] refine list of appointments.
- [x] use the javascript libary [flatpickr](https://flatpickr.js.org/) to select date and time requested for an appointment.

![image of appointment deletion](https://github.com/Journeygault/Timmins_V3/blob/master/imgs/calendar_appointment.jpg)

---

## Alexis Arevalo:

### FAQ & Category Features 

- [x] Users Can see all FAQ and Category data that is retrieved from the database.
        - User will not be able to Create, Update or Delete. They can only view the information.
- [x] Users Are able to search for data that matches the input by FAQ Question or FAQ Answer.
- [x] Admin Can see all FAQ and Category data that is retrieved from the database.
- [x] Admins Are able to search for data that matches the input by FAQ Question or FAQ Answer.
- [x] Admin Can Add new data into the database for FAQ and Category.
- [x] Admin Can Edit new data into the database.
        - When Adding or Editing FAQ data, the Admin has to select a given Category that is retrieved from the database.
        - All FAQs are associated with a certain Category - Seen through a foreign key assigned in the FAQ table that links to a specific Category Id.
- [x] Admin Can View the details about the specific data.
- [x] Admin Can Delete data from the database.
- [x] Validation is managed for all inputs with JS.
- [x] Styling added.

### Donation Feature

- [x] Users Can Add new data into the database for Donation and Event.
- [ ] Donation date is self inputed with the recent date upon submiting.
- [x] Users Can see all the events they made a donation to.
- [x] Admin Can see all the data that is retrieved from the database (except users card information).
- [x] Admin Can Edit new data into the database.
        - When Adding or Editing Donation data, the Admin has the option to select a given Event that is retrieved from the database. /- This is only done when a user confirms they've made a mistake on one of the inputs.
        - All Donations are associated with a certain Event - Seen through a foreign key assigned in the Donation table that links to a specific Event Id.
- [ ] Admin Can Delete data from the database.
- [ ] Admin Can View the details about the specific data.
- [ ] Validation is managed for all inputs with JS.
- [ ] Styling added.

### Page Design

- [x] Redesigned the footer and header with styling CSS.
- [x] Redesigned the Index page with styling CSS

- @Jorneygault /- Assisted retrieving the database.
---

## Journey Gault
### NewsItem:

- [x] Can see all the data that is retrieved from the database.
- [x] Basic Crud
- [x] Image upload
- [x] User Id functioning as forign key
- [ ] Styling
- [x] Razor Validation
- [x] Admin Only for create,update and delete
- [x] Different layout for admins and users

### Events:

- [x] Can see all the data that is retrieved from the database.
- [x] Basic Crud
- [x] Image upload
- [x] User Id functioning as forign key
- [x] Sponsers forign key (one to many)
- [ ] styling
- [x] Razor Validation
- [x] Admin Only for create,update and delete
- [x] Different layout for admins and users

### People who assisted Debuging my work:
-Sandra Kupfer: Assisted debugging of my image upload feature for 30 min
-Sandra Kupfer: Assisted debugging one to many relationship for 15 min
-Frank-Yves Cheuzem: Assisted Debugging Foring Key one to many relationship 30 min

---

## Adam Galek

### Ticket Feature:

#### Basic Crud
- [x] Users can create on the Ticket table
- [x] Users can read on the Ticket table
- [x] Users can update on the Ticket table
- [x] Users can delete on the Ticket table

#### Qualitative
- [ ] Pages are styled and responsive
- [ ] Code is commented

#### To Add
- [ ] Different Priviledges for admins and users for Tickets

---

## Nisarg Chauhan

### Job Posting
- [x] Admins can create a job post
- [x] Admins can read a job post
- [x] Admins can update a job post
- [x] Admins can delete a job post
- [ ] Department ID as foreign key
- [ ] Style the page

### Department
- [x] Admins can create a department
- [x] Admins can read department details
- [x] Admins can update department details
- [x] Admins can delete a department
- [ ] Style the page

---

## Steven Le

### Review Feature
- [x] User can see reviews
- [x] User can create a review
- [x] Admin can update review
- [x] Admin can delete a review
- [x] Pagination


### Bill Payment Feature
- [x] Admin can Create Bill
- [x] Admin can Update Bill
- [x] Admin can Delete Bill
- [x] Pagination

### To Add:
- [x] Connect to User Table both Review + Bill Feature

NOTE: Journey helped get inital migration working for my features.



