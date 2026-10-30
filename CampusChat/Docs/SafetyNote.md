# Safety questions
I have taken into account the GDPR rules when reflecting on this subject.
Many safety questions are formulated as if they are implemented. 

### 1. Personal Data Stored by the Product

The product stores the following personal data:

- Username
- First name
- Last name
- Study programme
- Start year
- Graduation date
- Profile introduction
- Forum posts
- Direct messages
- Thread participation records
- Audit logs after graduation
- Account identifiers

The product only stores personal data that is necessary for providing forum and messaging services. 

---

### 2. Direct Chat Retention

Direct chats are kept for **three years after the student has graduated**.
Only the audit role can access these messages after the user has graduated. This ensures the 
recent conversations can be accessed, and old personal data is eventually removed, in line
with GDPR's storage limitation principle. 

---

### 3. What Happens When a Student Leaves the College?

#### Posts

The student's posts remain visible so that discussion threads continue to make sense 
for other users. The profile is archived and other users will only see *User* : "Post".

#### Profile

The student's account is marked as graduated and archived, and only an audit can view it.
The profile is permanently deleted after the retention period expires unless there is a legal reason to keep it.

---

### 4. Abuse in a thread

There is a block to restrict sending posts or messages with offending language. Users 
also have the ability to report posts or messages. Any teacher can remove inappropriate posts, but
audits have the final say. 

---

### 5. Private direct messages

A user can only load the chats they are in themselves. If another user tries to access a chat
through the url, they get a message it is not valid. As of now, any teacher can choose to have
an audit role. This should be reconsidered when other members of faculty can create a user, and an
audit role may be given only to someone in the administration, for example. The product does not
tell the users an audit role can read their direct chat, but since this is a school platform it is 
implied (and probably stated in some legal form the student have seen once).

---

### 6. One thing that the product must never log

Contents of private messages. In the application log, it could reveal sensitive information when 
troubleshooting and monitoring. 

---

### 7. Private threads

The code is stored in the database together with the thread info and posts. When a user tries to
access a private thread, the entered password is cross-referenced with the database. A user can only join a private thread after entering the password. 
If a user has joined a private thread, they do not need to enter the password every time. 