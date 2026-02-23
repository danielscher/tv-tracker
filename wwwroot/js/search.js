export function searchMediaOnInput(inputId, resultContainerId, handlerUrl, redirectUrl, hiddableContainerId ){
    // antiforgery token.
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    // dom elements
    const searchBox = document.getElementById(inputId);
    const resultsContainer = document.getElementById(resultContainerId);
    const hiddenContent = document.getElementById(hiddableContainerId);

    let timeout = null;

    searchBox.addEventListener("input",async (event) => {

        clearTimeout(timeout);
        let query = event.target.value;
        timeout = setTimeout(async () => {

            const response = await fetch(handlerUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": token
                },
                body: JSON.stringify(query)
            });
            

            const data = await response.json();

            // toggle page contents to display search results.
            if (hiddenContent){
                if (data.length > 0) {
                    hiddenContent.classList.add("hidden");
                }else{
                    hiddenContent.classList.remove("hidden");
                }
            }

            resultsContainer.innerHTML = "";

            data.forEach(item => {
                const img = document.createElement("img")
                img.src = item.mediaInfo.posterPath;

                const li = document.createElement("li");
                li.className = "media-card";
                li.appendChild(img);

                const anchor = document.createElement("a")
                anchor.className = "media-card-link";
                anchor.href = `${redirectUrl}/${item.id}`
                anchor.appendChild(li)

                resultsContainer.appendChild(anchor);
            });

        }, 300); // wait 300ms after typing stops
    });
}