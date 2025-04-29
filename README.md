 WebChatApp
Project Description:
WebChatApp is a real-time chat application built with ASP.NET Core MVC and SignalR, following the Clean Architecture principles to ensure scalable, maintainable, and testable code.
The app supports user authentication, one-to-one and group messaging, end-to-end message encryption, chat list previews with unread message counters, and real-time user status tracking (online/offline with last seen) — all developed with adherence to SOLID principles and the Unit of Work pattern.

🔑 Key Features:
• Account Management:
Register new users.

Login with secure authentication.

Edit profile (username, photo, bio).

Permanently delete account.

• Messaging System:
Send/receive hybrid-encrypted messages (RSA + AES).

Edit or delete messages.

Display chat list:

Last message preview.

Unread message count.

Real-time user status (online/offline, last seen).

Search users in chat.

Mark messages as read when opening chat.

• Group Messaging:
Send messages in group chats.

Edit or delete group messages.

Show unread message count per group.

Real-time updates via SignalR.

• Group Management:
Create or delete groups.

Add/remove group members.

Promote/demote members to/from admins.

Full group member and admin control.

View group size (members and admins).

Join or leave a group.

• Security & Encryption:
Hybrid encryption using RSA + AES for all messages.

Decrypt single messages or lists of messages securely.

• Error Logging:
Log all exceptions with full details (message, stack trace, method name).

• Additional Services:
Track real-time user presence (online/offline).

Manage user context via UserContextService.

Real-time syncing using SignalR.

Store essential data as JSON for flexible caching/config.

🛠️ Architecture Highlights:
SOLID principles: Clean, extensible, and maintainable codebase.

Clean Architecture: Clear separation of concerns across layers.

Unit of Work pattern: Efficient DB transaction management.

DTOs: Safe and optimized data handling between DB and UI.

