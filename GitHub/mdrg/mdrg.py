import os


def markdownReleaseGenerator():
    title:str = str(input("Release title (RELEASE.MAJOR.MINOR.PATCH): "))
    
    if os.path.exists(f"GitHub/mdrg/releases/{title}.md"):
        print("Cannot create a new file with the name of an existing file.")
        return 

    changes = []

    change = "change"

    while True:
        change = str(input("Enter change (Leave blank to break out): "))
        
        if change == "":
            break
        
        changes.append(change)

    os.makedirs("GitHub/mdrg/releases", exist_ok=True)

    with open(f"GitHub/mdrg/releases/{title}.md", "x") as file:
        finalText:str = f"# Changelog | v{title}\n";

        for singular_change in changes:
            finalText += (f"\n- {singular_change}")
        
        file.write(finalText)

    print(f"File '{title}.md' created in releases/")

markdownReleaseGenerator()