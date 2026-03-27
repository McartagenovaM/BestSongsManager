# Copilot Instructions

This repository contains an ASP.NET Core MVC application built with Razor views.

## Project Purpose
Build a simple web application to manage favorite songs.
The app must use a JSON file as its data store.
Do not use any relational or NoSQL database.
Do not add Entity Framework or database migrations.

## Technical Stack
- ASP.NET Core MVC
- Razor Views
- Bootstrap for UI styling when useful
- System.Text.Json for serialization
- Dependency Injection for service registration

## General Architecture
Follow a simple MVC structure:
- Models: domain models used by the application
- Services: business logic and JSON file persistence
- Controllers: coordinate requests and responses
- Views: Razor UI for user interactions

Keep the design simple, maintainable, and easy to extend.

## Data Storage Rules
- Persist song data in a JSON file located at `Data/songs.json`
- The application must work without a database
- All file reading and writing must be handled in the service layer
- Controllers must never read or write JSON files directly
- Use async methods for file access whenever possible
- Handle missing files gracefully
- Handle malformed JSON gracefully

## Coding Rules
- Use English for:
  - code
  - class names
  - method names
  - property names
  - comments
  - commit messages
- Keep methods small and readable
- Prefer clear code over clever code
- Follow standard .NET naming conventions
- Avoid unnecessary abstractions
- Add only the minimum complexity needed for the current requirement

## Functional Scope
The application should support:
- Listing songs
- Viewing song details
- Creating songs
- Editing songs
- Deleting songs
- Marking and unmarking favorites
- Searching by title, artist, album, or genre
- Filtering favorites

## Domain Model
A song should typically include:
- Id
- Title
- Artist
- Album
- Genre
- Year
- IsFavorite
- Notes
- Url or FilePath if needed later

## Service Design
Create:
- `ISongService`
- `JsonSongService`

The service should be responsible for:
- Loading songs from the JSON file
- Saving songs to the JSON file
- Returning all songs
- Returning a song by id
- Creating a new song
- Updating an existing song
- Deleting a song
- Toggling favorite status
- Searching and filtering songs when requested

## UI Guidelines
- Use standard Razor MVC views
- Keep the interface clean and simple
- Prefer Bootstrap components when useful
- Avoid adding React, Angular, Vue, or other frontend frameworks
- Show favorite status clearly in list and detail views
- Keep forms simple and user-friendly

## Controller Guidelines
Create a `SongsController` as the main controller for the feature.
Controllers should depend on `ISongService`.
Keep controllers thin.
Put file access and data manipulation logic in the service layer.

## Error Handling
- Validate user input
- Return friendly error messages in the UI
- Avoid application crashes due to invalid JSON or missing files
- Use safe defaults when reasonable

## Project Evolution Rules
- Keep the app ready for future extension
- Do not introduce a database unless explicitly requested
- Do not overengineer
- When changing code, keep consistency across models, services, controllers, and views
- Prefer end-to-end working implementations

## When Generating Code
When generating or modifying code:
1. Respect the MVC structure already in the project
2. Do not introduce database-related packages
3. Update all affected files consistently
4. Keep comments short and useful
5. Make safe assumptions when requirements are incomplete
6. Preserve a beginner-friendly project structure