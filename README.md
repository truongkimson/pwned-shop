# pwned-shop

Pwned Shop is an online shopping website which sells Steam games.

# User account
Please use the following user account to test features
Email: dprice@msn.com
Password: HelloBY90

# App database
The app is configured to delete and generate a new db every time
the app is run, for the purpose of testing.

If you want to disable this, proceed to comment out this line in Data/DbInitializer.cs
16             db.Database.EnsureDeleted();

# Emailing features
Currently, we're using a developer account on MailGun to send out receipt emails.
Developer account is meant for testing so only emails registered with MailGun can
receive receipt emails.

If you would like to test this feature with your own email, please send your email
to truongkimson@u.nus.edu 
