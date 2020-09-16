# Database Concurrency Demos
This project demos different implementations of concurrency strategies in C#.

## Optimistic Concurrency
The optimistic concurrency demos are found in the *OptimisticConcurrencyWithTimestampCheck.cs* and *OptimisticConcurrencyWithValueCheck.cs* file respectively.



## Pessimistic Concurrency 
The pessimistic concurrency demos are found in the *ImplicitTransactionWithIsolationLevel.cs* and the *ExplicitTransactionWithIsolationLevel.cs* file respectively.

* **ReadUncommitted**  
Does not lock the records being read. This means that an uncommitted change can be read and then rolled back by another client, resulting in a local copy of a record that is not consistent with what is stored in the database. This is called a dirty read because the data is inconsistent.
* **ReadCommitted**  
Locks the records being read and immediately frees the lock as soon as the records have been read. This prevents any changes from being read before they are committed, but it does not prevent records from being added, deleted, or changed by other clients during the transaction, resulting in non-repeatable reads or phantom data. This is the default isolation level.
* **RepeatableRead**  
Locks are placed on all data that is used in a query, until the transaction completes. This ensures that the data being read does not change during the transaction. Prevents non-repeatable reads but phantom rows are still possible.
* **Serializable**  
Locks the entire data set being read and keeps the lock until the transaction completes. This ensures that the data and its order within the database do not change during the transaction by other users that updates or inserts rows into the dataset. This is the most restrictive isolation level.
* **Snapshot**  
Reduces blocking by storing a version of data that one application can read while another is modifying the same data. Indicates that from one transaction you cannot see changes made in other transactions, even if you requery.
* **Chaos**  
The pending changes from more highly isolated transactions cannot be overwritten.

## Implicit and Explicit Transactions
The difference between implicit and explicit transactions are that an implicit transaction is autocommitted and an explicit transaction are not. That means that the explicit transaction has a begin, commit and rollback command, while the implicit transaction has not. 

The TransactionScope class provides a simple way to mark a block of code as participating in a transaction, without requiring you to interact with the transaction itself. A transaction scope can select and manage the ambient transaction automatically. Due to its ease of use and efficiency, it is recommended that you use the TransactionScope class when developing a transaction application.

The CommittableTransaction class provides an explicit way for applications to use a transaction, as opposed to using the TransactionScope class implicitly. It is useful for applications that want to use the same transaction across multiple function calls or multiple thread calls. Unlike the TransactionScope class, the application writer needs to specifically call the Commit and Rollback methods in order to commit or abort the transaction.

Here you can read more about implementing [implicit][2] and [explicit][1] transactions 

> Examples on the use of the TransactionScope and SqlTransaction classes is implemented in the *ImplicitTransactionWithIsolationLevel.cs* and *ExplicitTransactionWithIsolationLevel.cs* files.

## Pessimistic Concurrency Demo 
Try these steps to test the impact on the different isolation levels:
1. In the *Program.cs* file uncomment line 14-15 in the **Main()** method:  

   ```csharp
        static void Main()
        {
            new ImplicitTransactionWithIsolationLevel()
                 .Execute();

            //new ExplicitTransactionWithIsolationLevel()
            //    .Execute();

            //new OptimisticConcurrencyWithTimestampCheck()
            //    .Execute();

            //new OptimisticConcurrencyWithValueCheck()
            //    .Execute();
        }
   ```
1. Open the *ImplicitTransactionWithIsolationLevel.cs* file 
1. In line 20, set the isolation level to ReadCommitted and run the application. 
1. When you see the message: *"Data is read from database, try and change data from another client and continue this operation by pressing &lt;ENTER&gt;..."*, Open the Animal table in another data editor and try and change any value in the **Name** column. What happens?  
1. Now change the isolation level to RepeatableRead and repeat the previous step. What is the difference from the ReadCommitted isolation level?
1. Repeat the demo using an explicit transaction as implemented in the *ExplicitTransactionWithIsolationLevel.cs* file. 

## References
* [Article about transactions and concurrency in ADO.NET][3]
* [Article about isolation levels][4]


[1]: https://docs.microsoft.com/en-us/dotnet/framework/data/transactions/implementing-an-explicit-transaction-using-committabletransaction
[2]: https://docs.microsoft.com/en-us/dotnet/framework/data/transactions/implementing-an-implicit-transaction-using-transaction-scope
[3]: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/transactions-and-concurrency
[4]: https://docs.microsoft.com/en-us/sql/connect/jdbc/understanding-isolation-levels?view=sql-server-2017
