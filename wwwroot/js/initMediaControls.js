/**
 * Initializes the functionality of MediaControls components.
 */
export function initMediaControls(antiForgeryToken) {
    const mediaId = document.getElementById("media-controls").dataset.mediaId;
    initRateButtons(mediaId,antiForgeryToken);
    initWatchLaterButton(mediaId,antiForgeryToken);
}

/**
 * Rate button toggle visibility of the rating inputs.
 * Rate inputs send a POST request to update rating.
 */
function initRateButtons(mediaId,token) {
     // Toggle visibility of rating inputs.
    const RatingButton = document.getElementById("rating-button");
    const rateContainer = document.getElementById("rating-container");
    RatingButton.addEventListener("click", 
        () => {
            if (rateContainer.classList.contains("hidden")) {
                rateContainer.classList.remove("hidden");
            } else {
                rateContainer.classList.add("hidden");
            }
        }
    );

    // Add listeners to each radio that send a POST request.
    document.querySelectorAll("input[type='radio']").forEach(radio => {

        radio.addEventListener("change", async () => {

            const rating = parseInt(
                document.querySelector('input[name="rating"]:checked').value
            );

            const formData = new URLSearchParams();
            formData.append("rating", rating);

            const response = await fetch(`?handler=Rate&mediaId=${mediaId}`, {
                method: "POST",
                headers: {
                    "RequestVerificationToken": token
                },
                body: formData
            });

            if (!response.ok) {
                return;
            }

            const data = await response.json();
        });
    });
}

/**
 * WatchLater button sends a post request to toggle the WatchStatus of media.
 */
function initWatchLaterButton(mediaId, token) {

    const watchButton = document.getElementById("toggle-watch-later");

    watchButton.addEventListener("click", async ()=> {
        await fetch(`?handler=ToggleWatchLater&mediaId=${mediaId}`, {
            method: "POST",
            headers : {"RequestVerificationToken":token}
        });
    });

}