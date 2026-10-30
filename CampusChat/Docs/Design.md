# Design Notes & Architecture Decisions

**Date:** 29.10.26
**Author:** Kristiane Olsen
**Context:** A school chat platform where students and teachers can create profiles,
send direct messages and posts in threads. 

---

## I. Layout and Navigation

*   **Layout Strategy:** A single-layout based chat service with different pages 
depending on the function.
*   **Screen Count/Structure:** All the pages use the same structure, so it appears all the 
pages are on the same screen. The different pages have their own functions, and includes: Messages, Threads, Profiles and Login.
*   **Rationale:** I wanted to have an intuitive application where it is easy to navigate to the 
different functions. 

## II. Routing and URLs

*   **Route Definitions:**
  *   `/` (Home): Redirects to /threads. Activated by clicking CampusChat in the navigation bar
  *   `/threads`: Selectable list of all threads
  *   `/threads/{threadTitle}`: Shows the content of the chosen thread
  *   `/threads/joinedThreads`: A list of selectable threads the user has joined
  *   `/threads/joinedThreads/{threadTitle}`: The content of the thread from the joined threads list
  *   `/profiles`: A selectable list of all profiles (alumni only for audits in audit mode)
  *   `/profiles/{username}`: Shows the users information, and an option to send a direct message, or edit the profile
 if it is the logged-in user. 
  *   `/messages`: A selectable list of all profiles the user has a direct message chat with
  *   `/messages/{username}`: Shows the direct message chat if there is an existing chat between the logged-in user and the
one in the url.
  *   `/messages/dm/{dmId}`: An audit can access an existing direct message chat between any two users.
  *   `/Login`: Shows a list of profiles available for login (non-alumni)
*   **Rationale:** I wanted to keep the paths as simple as possible, but also easily access for example
joined threads. I use username and thread title instead of id for a better overview and understanding of 
what the url means. When an audit reads a direct message chat, it uses its own path and the dm id. A non-audit
user cannot simply access this when trying the same url.

## III. State Management (Component vs. Service)

### Scoped services: 
  * **UserService** 
  * **AuditService**

The logged-in user details needs to be unique to the state, so the program can access the correct
data based on the user. The audit service as it is now also needs to be scoped, because this is unique to the user 
if they are an audit. The audit service could instead have been its own class, having more functionality,
and rather have the services inherit from it.

### Singleton services:
  * **ThreadService** 
  * **DirectMessageService**
  * **NotificationService**

These have functionality which needs to be updated across the users, to render new messages and increase the notification count.
A singleton service is nice for this purpose, as it uses the same instance across all users.

## IV. Data Persistence and Lifecycles

*   **Data Storage Strategy:** Since this is a small-scale application, I chose to store the data in .json. The UserService and ThreadService 
inherits from DbService to access the data. 
*   **Data Location Mapping:**
    *   Logged-in user: Saved in browser storage. Survives reloads and restarting the application.
    *   New posts and messages: New messages and posts are immediately stored in the database, which activates an event Action, 
    and refreshes the data so the new content appears immediately.

## V. Post Structure

*  **Id:** An unique GUID, to make it accessible for the IEntity interface. 
*  **Author:** Username of author
*  **Message:** Contents of the post
*  **Timestamp:** Date and time when the post was made
*  **ThreadId:** Which thread the post belongs to
*  **ParentPostId:** If the post is a reply to a different post

## VI. Thread Feature Configuration

The threads have a title, and the threads are set. In the future, teachers could make new threads as needed. 
There are no owners of any threads as of now.

## VII. Notification counts on threads
The user only gets a notification on the threads they have joined. There are many threads, and it would
be too cluttering to get notifications on every new post in every thread. I did not have time to implement already
joined threads for new users (for example Information from the School and other useful threads), and maybe a feature that
some of these threads cannot be left. 