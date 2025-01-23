
## Description
The **Enhanced Online Exam System** is a C#-based application designed for online exam management, providing both **student** and **admin** roles. It allows students to take exams, track their progress, and view a leaderboard. Admins can manage questions, approve pending ones, and export results as CSV files.

## Features
### For Students:
- **Login/Registration**: Secure login and registration system.
- **Start Exam**: Take exams in various categories, with questions displayed and evaluated in real-time.
- **Progress Tracking**: View past exam scores and average score.
- **Leaderboard**: View the top scorers and compare your performance.
  
### For Admins:
- **View All Questions**: View all questions in the question bank.
- **Add New Question**: Admins can add questions with options, categories, difficulty levels, and explanations.
- **Remove Question**: Admins can remove questions from the question bank.
- **Export Results**: Export exam results of all students into a CSV file for easy reporting.
- **Approve Questions**: Placeholder feature for managing pending questions.

## Requirements
- .NET 6 or higher
- Newtonsoft.Json (for managing user data and storing exam results)

## Setup

### 1. Clone the repository

```bash
git clone https://github.com/your-repository/enhanced-online-exam-system.git
```

### 2. Install dependencies

Ensure that you have .NET 6 or higher installed. You can check if it's installed by running:

```bash
dotnet --version
```

Install any required dependencies:

```bash
dotnet restore
```

### 3. Run the application

You can start the application by navigating to the project directory and running:

```bash
dotnet run
```

## Usage

### Login or Register

When you start the application, you'll be prompted to either log in or register:

- **Login**: Enter your username and password to access the system.
- **Register**: If you're a new user, you can register with a username and password.

### Student Role

Once logged in as a student, you can:

1. **Start Exam**: Choose a category and begin the exam.
2. **View Progress**: Check your past scores and the average score.
3. **View Leaderboard**: View the leaderboard with top scorers.
4. **Logout**: Log out of the system.

### Admin Role

Once logged in as an admin, you can:

1. **View All Questions**: View the full list of questions in the system.
2. **Add New Question**: Add new questions, specify options, categories, difficulty levels, and explanations.
3. **Remove Question**: Remove a question from the question bank.
4. **Export Results**: Export the scores of all students to a CSV file.
5. **Approve Questions**: Manage pending questions (currently a placeholder).

## File Structure

```
├── Program.cs                  # Main entry point
├── ExamSystem.cs               # Exam system logic and user management
├── Question.cs                 # Defines a question object
├── User.cs                     # Defines a user object
├── README.md                   # Project description and setup guide
```

## Example Output

After logging in as a student, you can:

1. Take an exam, answering questions and getting real-time feedback.
2. Check your progress report, including past scores and average scores.
3. View the leaderboard with the top-performing students.

As an admin:

1. View all questions in the question bank.
2. Add new questions with options, categories, and difficulty levels.
3. Export the results of all students to a CSV file.

