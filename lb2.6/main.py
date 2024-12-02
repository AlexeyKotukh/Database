from pymongo import MongoClient
from pprint import pprint

client = MongoClient("mongodb://localhost:27017/")
db = client["charity"]


def select_collection():
    print("\n--- Collections ---")
    print("1. Donors")
    print("2. Donations")
    print("3. Projects")
    print("4. Volunteers")
    print("5. VolunteerProjects")
    print("6. Exit")
    return input("Choose an action: ")


def create(collection_name):
    print(f"\n--- Create a document in collection: {collection_name} ---")

    collection = db[collection_name]
    sample_doc = collection.find_one()
    if not sample_doc:
        print(f"The collection '{collection_name}' is empty. Please add fields manually.")
        return
    fields = sample_doc.keys()
    document = {}
    for field in fields:
        if field == "_id":
            continue
        value = input(f"Enter value for '{field}': ").strip()
        if "," in value:
            document[field] = value.split(",")
        else:
            document[field] = int(value) if value.isdigit() else value
    insert_result = collection.insert_one(document)

    if insert_result.acknowledged:
        print(f"Document created successfully!")
    else:
        print("Failed to create document.")


def read(collection_name):
    print(f"\n--- Read documents from collection: {collection_name} ---")
    collection = db[collection_name]
    documents = collection.find()
    for doc in documents:
        pprint(doc)
        print('\n')


def update(collection_name):
    print(f"\n--- Update document in collection: {collection_name} ---")

    collection = db[collection_name]

    field = input("Enter field name to search by: ").strip()
    value = input(f"Enter value for {field}: ").strip()

    try:
        value = int(value)
    except ValueError:
        pass

    try:
        document = collection.find_one({field: value})
        if not document:
            print(f"No document found with {field} = {value}")
            return

        print("\nCurrent document:")
        pprint(document)
        updated_fields = {}
        for key, old_value in document.items():
            if key != "_id":
                new_value = input(f"Enter new value for '{key}' (leave empty to keep current value): ").strip()
                if new_value:
                    updated_fields[key] = int(new_value) if new_value.isdigit() else new_value
                else:
                    updated_fields[key] = old_value

        if updated_fields:
            update_result = collection.update_one(
                {field: value},
                {"$set": updated_fields}
            )

            if update_result.modified_count > 0:
                print("\nDocument updated successfully!")
            else:
                print(f"Failed to update the document with {field} = {value}.")
        else:
            print("No fields were updated.")
    except Exception as e:
        print(f"Error: {e}")


def delete(collection_name):
    print(f"\n--- Delete document(s) from collection: {collection_name} ---")
    collection = db[collection_name]

    print("1. Delete a specific document by field")
    print("2. Delete all documents in the collection")
    choice = input("Choose an action: ").strip()

    if choice == "1":
        field = input("Enter field name to identify the document: ").strip()
        value = input(f"Enter value for '{field}': ").strip()

        try:
            value = int(value) if value.isdigit() else value
            result = collection.delete_one({field: value})

            if result.deleted_count > 0:
                print(f"Document with {field} = {value} was successfully deleted.")
            else:
                print(f"No document found with {field} = {value}.")
        except Exception as e:
            print(f"Error during deletion: {e}")

    elif choice == "2":
        confirmation = input("Are you sure you want to delete all documents? (y/n): ").strip().lower()

        if confirmation == "y":
            try:
                result = collection.delete_many({})
                print(f"Deleted {result.deleted_count} documents from the collection.")
            except Exception as e:
                print(f"Error during deletion: {e}")
        else:
            print("Operation canceled.")

    else:
        print("Invalid choice. Returning to the CRUD menu.")


def main():
    try:
        while True:
            collection_choice = select_collection()
            collections = {
                "1": "Donors",
                "2": "Donations",
                "3": "Projects",
                "4": "Volunteers2",
                "5": "VolunteerProjects"
            }
            if collection_choice in collections:
                collection_name = collections[collection_choice]
                while True:
                    print(f"\n--- CRUD menu for {collection_name}---")
                    print("1. Create")
                    print("2. Read")
                    print("3. Update")
                    print("4. Delete")
                    print("5. Back to choose another collection")
                    print("6. Exit")
                    action = input("Choose an action: ")
                    if action == "1":
                        create(collection_name)
                    elif action == "2":
                        read(collection_name)
                    elif action == "3":
                        update(collection_name)
                    elif action == "4":
                        delete(collection_name)
                    elif action == "5":
                        break
                    elif action == "6":
                        print("Clossing...")
                        exit()
                    else:
                        print("Error!.")
            elif collection_choice == "6":
                print("Clossing...")
                break
            else:
                print("Error!")

    except KeyboardInterrupt:
        print("\nClossing...")
        exit()


if __name__ == "__main__":
    main()
