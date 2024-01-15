## Affiliate Design

### Core Diagram

1. We can't shared the customer information. Therefore, we isolated it for each affiliate
2. The customer data must be consolidated inside an internal database  

---
```mermaid 
---
title: Affiliate Diagram 
---
classDiagram
    Affiliate --> AffiliateCustomer
    Affiliate --> AffiliateMedia
    Affiliate --> AffiliateDetails

    class AffiliateCustomer{
        +Id
        +AffiliateId
        +FullName
        +Email
        +Gender
        +BithDate
        +TotalPurchase
        +AvgTicket
        +Orders[]
    }
    class Affiliate{
        +Id
        +FullName
        +Email
        +SocialMedias[]
        +AffiliateDetails
    }
    class AffiliateMedia{
        +AffiliateId
        +ReferenceId
        +MediaType
    }
    class AffiliateDetails{
        +AffiliateId
        +TaxDetails
        +Identifier
        +IdentifierType
    }

    class CustomerInvite{
        
    }


```
### Affilite Customer Creation
```mermaid
flowchart TD
    Start[New Affiliate Customer] -->|CustomerCreated Event|Kafka
    Kafka[[Kafka]]-->StreamProcessor
    StreamProcessor-->|Unify the Customer |DB[(OLAP database)]
```


### CustomerData  

Used for Data Science and creating campaigns.
suggestions: Apache Druid, Pinot, Microsoft Analysis Services

```mermaid 
---
title: Customer OLAP Database
---
classDiagram
    Customer --> CustomerTags
    Customer --> CustomerInterests
    Customer --> CustomerInsights
    Customer --> CustomerOrders
    class Customer{
        +Gender
        +Age
        +AvgTicket      
    }
    class CustomerInterests{

    }
    class CustomerInsights{
        +ProductsVisited[]
        +More...
    }

    class CustomerOrders{

    }


```
### Tables:

- Affiliate:
- AffiliateDetail
- AffiliateSocialMedia
- AffiliateBankAccount
- AffiliateAddress
- AffiliateCustomer
- AffiliateCustomerTag
- Sales.OrderHeader
- Sales.Product
- Sales.OrderItem
- AppUser


### Marketing Management Insights

- Sales
    - Follow up System
        - Maintaining contact with potential customers after an initial interaction.     
    - Sales Funnel
        - Untouncehd
        - Contact mad (Leads)
        - Qualified Leads
        - Demo
        - Negotication
        - Deals Won

