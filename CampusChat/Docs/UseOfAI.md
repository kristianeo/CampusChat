# Use of AI

I asked my AI to send me a summary of what we discussed the last 3 weeks. I do not feel the list was complete, even after like 7 prompts. 
I have added the relevant topics the AI listed here, along with relevant information from my head. There have been many conceptual
discussions not listed here, and also minor bugs/logic errors (like a mixed up boolean) discussions used for troubleshooting. 

### DbService 
In the beginning, I had some code for ThreadService in another class, and when I made UserService I figured I needed a generic parent class.
I asked AI to make this for me using the code I had already written, which is seen in DbService. I wrote all the logic, AI made it generic so 
both classes could inherit from it. 

### Parent/child communication
I used AI in the beginning to understand the concept of the components sharing data, and showed it my code to tell me why
they didn't communicate as I expected. It did not suggest code, but explained to me why it was wrong and what principles
I should follow to make it work. 

### Lifecycle Methods

I have had many discussions about the lifecycles of OnInitialized/OnInitializedAsync/OnParametersset/OnAfterRenderAsync,
to understand when they run and how to access the data at the right time. 

### HTML

I wrote all HTML myself, but asked AI for help when I did not know what to use to get it exactly how I wanted it to look. 
I tested the suggestions until I got an arrangement that I liked. I then asked AI to make a generic template for me containing that code.
This is seen as the SplitPanel component. 

### Identify graduate users 

I asked for the best way to identify graduate users, I added this to Profile as suggested: 

    bool Graduated,
    DateTime? GraduationDate

AI suggested to create a method to calculate this, but for the scale of this assignment I found it best to
calculate this statically within the record. 

### Audit User Visibility

Audit users should see all users, while normal users should only see active students. I used this suggested addition
to my code (shown with the arrow):

```csharp
_profiles.Where(p =>
    (_search == "" ||
     p.UserName.Contains(
         _search,
         StringComparison.OrdinalIgnoreCase))
    &&
    ((User?.Audit ?? false) || !p.Graduated)  <------ 
);
```

---

### Ambiguous Routing

The following routes conflict:

```razor
@page "/Messages/{DmId}"
@page "/Messages/{Username}"
```

#### Suggested Solution

```razor
@page "/Messages"
@page "/Messages/Dm/{DmId}"
@page "/Messages/User/{Username}"
```

I chose to keep "/Messages/Dm/{DmId}" for audit use, as both DmId and Username are strings.
I didn't find it necessary to keep the /User/ path for normal dms, as it is easier and more readable to directly use
Messages/{Username}.

---

### Styling

- Logic is working correctly, but Bootstrap styling overrode custom styling.
- I added some CSS suggested by AI shown under

#### Suggested CSS

```css
.selected-post {
    border: 1px solid cornflowerblue !important;
}
.disabled {
    pointer-events: none;
    opacity: 0.65;
}
```

### Null-safe contains
AI taught me about this concept, which can then be seen throughout the assignment: _thread.JoinedUsers?.Contains(User?.UserName) ?? false.

### Threads
The first 5 threads are created by me, the rest is AI. 

## Impact of AI use 
In some aspects it has made the process more efficient by automating repetitive tasks and restructuring code I have already written.
It has also made the process of learning parent/child communication and HTML a lot quicker. On the downside, it lowers the
threshold of asking for help, compared to taking the time to troubleshoot yourself. I have tried to use it primarily as a learning and 
productivity tool.