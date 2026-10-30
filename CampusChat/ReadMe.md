# CampusChat

## Description

CampusChat is a university-focused communication platform built with Blazor. 
The application supports discussion threads, direct messaging between users, 
profile management, and an audit role for reviewing content.

The goal of the application is to provide a place where students and faculty can 
communicate with all the information in one place, while supporting privacy requirements, 
moderation, and data retention policies.

---

## Features

### User Accounts

- User profiles
- Study programme information
- User introductions
- Graduation tracking

### Threads

- Public threads
- Private threads
- Thread membership management

### Direct Messages

- One-to-one conversations
- Message history
- Unread message tracking

### Audit Features

- Audit users can view graduated students
- Audit users can review direct messages
- Audit users can access private threads

### Data Retention

- Direct messages are archived after the retention period
- Archived content is hidden from normal users
- Old archived content can be deleted automatically

---

## Technology Stack

### Backend

- ASP.NET Core
- Blazor

### Frontend

- Bootstrap 5
- HTML
- CSS

### Language

- C#

---

## Requirements

The following software must be installed before running the application:

### Required

- .NET SDK 8.0 or newer
- A modern web browser
- Git

---

## Installation

### 1. Clone Repository

```bash
git clone https://github.com/kristianeo/CampusChat.git
```

### 2. Enter Project Directory

```bash
cd CampusChat
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Build Application

```bash
dotnet build
```

### 5. Run Application

```bash
dotnet run
```

### 6. Open In Browser

Navigate to:

```text
https://localhost:5112
```

Use the URL displayed by the application.

---

## Direct Message Retention Policy

The application follows the following retention process:

```text
Active Conversation
        ↓
Archived Conversation
        ↓
Deleted Conversation
```

Current retention policy:

- Direct messages are kept for 3 years after the student had graduated.
- Conversations are archived before deletion.
- Archived conversations are hidden from normal users.

---

## Known Issues

- Automated cleanup of archived conversations.
- Additional audit logging.
- Advanced moderation tools.
- Additional reporting functionality.
- New students/faculty automatically subscribe to relevant threads
- Own module for saved posts used by lecturers

---

## Current Status

The application is functional and can be built and run on a clean machine using the instructions above.

The following core functionality is currently available:

- User profiles
- Public threads
- Private threads
- Direct messaging
- Audit permissions
- Graduation tracking
- Data retention support

Some moderation and automation features remain under development.