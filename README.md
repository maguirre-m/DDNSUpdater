# DDNSUpdater


### Purpose

This app can server as a Dynamic DNS record updater to keep ip addresses up to date when the hosts are behing a network connection without a static public IP Address (such as NAT networks and other shared resource mappings implemented by non enterprise ISP packages)-

### Prerequisites

- This app currently supports CloudFlare as a DNS record manager and requires a SQL database to store and retrieve the secrets used to update the target DNS record.
    - Provide the connection string details to the database where the CloudFlare secrets are stored, the database should contain a table named CloudFlareSettings with the following schema: 

    ```
    CREATE TABLE [dbo].[CloudFlareSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Value] [varchar](1000) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedBy] [varchar](255) NULL,
	[Valid] [bit] NOT NULL
    ) ON [PRIMARY]
    GO

    ALTER TABLE [dbo].[CloudFlareSettings] ADD  CONSTRAINT [DF_CloudFlareSettings_Valid]  DEFAULT ((1)) FOR [Valid]
    GO
    ```
    - Ensure the table has at least two records with the following info: 
      - Name: "ZoneId", Value: [Zone Id value from CloudFlare]
      - Name: "ApiKey", Value: [ApiKey from CloudFlare for reading / updating DNS info on this zone]
    - The appSettings file has a CloudFlare section where you can set the name of the target DNS record to update.
    
### Functionality

- The app will run and immediately review the settings to use, then it will attempt to fetch the current IP address and find if it matches the last IP address on file, if there are no changes, the app will log the status and shut down.

If the IP address has changed, the app will attempt to update the DNS record in CloudFlare, logging any potential issues if it fails to do so.


### Pending items

Basic testing coverage is included, however future versions could support other domain providers and features.
In order to keep the record up to date, it is recommended to run this program on a scheduled basis (OS scheduled task or otherwise). Future implementations could include an internal scheduler to execute the logic periodically.

### Remarks

This solution uses the CQRS pattern through the MediatR implementation to locate, instantiate and use functionality service providers.

