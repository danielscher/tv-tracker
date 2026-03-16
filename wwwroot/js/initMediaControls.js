/**
 * Initializes the functionality of MediaControls components.
 */
export function initMediaControls(antiForgeryToken) {
    const tmdbId = document.getElementById("media-controls").dataset.tmdbId;
    initRateButtons(tmdbId,antiForgeryToken);
    initWatchLaterButton(tmdbId,antiForgeryToken);
    initMarkAsWatchedButton(tmdbId, antiForgeryToken);
}

/**
 * Rate button toggle visibility of the rating inputs.
 * Rate inputs send a POST request to update rating.
 */
function initRateButtons(tmdbId,token) {
     // Toggle visibility of rating inputs.
    const ratingButton = document.getElementById("rating-button");
    const rating = ratingButton.dataset.rating;
    initRatingButtonContents(rating == "" ? null : rating);
    const rateContainer = document.getElementById("rating-container");
    ratingButton.addEventListener("click", 
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
            postChangeRating(tmdbId,rating,token);
            initRatingButtonContents(rating);
        });
    });

    // clear button
    const clearRatingButton = document.getElementById("clear-rating");
    clearRatingButton.addEventListener("click", ()=> 
        {
            const response = postChangeRating(tmdbId,null,token);
            initRatingButtonContents(response.rating);

            // deselect current rating selection
            const radios = document.querySelectorAll('input[name="rating"]');
            radios.forEach(radio => {
                radio.checked = false;
            });

            // hide rating selection
            // const rateContainer = document.getElementById("rating-container");
            // rateContainer.classList.remove("hidden");
            // rateContainer.classList.add("hidden");
        }
    )
}

async function postChangeRating(tmdbId,rating,token) {
    const formData = new URLSearchParams();     
    formData.append("rating", rating);

    const response = await fetch(`?handler=Rate&tmdbId=${tmdbId}`, {
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
    return data;
}

/**
 * updates the button ui based on the rating score.
 * @param ratingScore current score.
 */
function initRatingButtonContents(ratingScore) {
    // remove old contents
    const ratingButton = document.getElementById("rating-button");
    const currentContents = document.getElementById("rating-button-contents");
    ratingButton.removeChild(currentContents);

    // create new contents
    const buttonContents = document.createElement("div");
    buttonContents.id = "rating-button-contents"
    const img = document.createElement("img");
    img.className="rating-img"

    let msg = document.createElement("span");
    if (ratingScore != null) {
        img.src = "/assets/popcorn_lowdetail_2.svg";
        msg.textContent = ratingScore
        msg.className = "score";
    } else {
        img.src = "/assets/popcorn_box_empty.svg";
        img.style.transform = "scaleX(-1)";

        msg = document.createElement("div");
        const span1 = document.createElement("span");
        span1.textContent = "Rate";

        const span2 = document.createElement("span");
        span2.textContent = "What did you think?";
        span2.className = "secondary-message";
        msg.appendChild(span1);
        msg.appendChild(span2);
        msg.className = "rate-message";
    }

    buttonContents.appendChild(img);
    buttonContents.appendChild(msg);
    ratingButton.appendChild(buttonContents);
}

/**
 * WatchLater button sends a post request to toggle the WatchStatus of media.
 */
function initWatchLaterButton(tmdbId, token) {
    const watchButton = document.getElementById("toggle-watch-later");
    const active = watchButton.dataset.active
    updateWatchLaterButtonContents(active)
    watchButton.addEventListener("click", async ()=> {
        const response = await fetch(`?handler=ToggleWatchLater&tmdbId=${tmdbId}`, {
            method: "POST",
            headers : {"RequestVerificationToken":token}
        });
        const data = await response.json()
        updateWatchLaterButtonContents(data.saved)
    });
}

function updateWatchLaterButtonContents(active) {
    const msgContainer=document.getElementById("watchlater-button-msg")
    const img = document.getElementById("watch-later-btn-image")
        const span = document.createElement("span")
        const span2 = document.createElement("span")
        span2.className = "secondary-message";
        if (active) {
            img.src = "/assets/icons/bookmark-full.svg"
            span.textContent = "Saved"
            span2.textContent = "in watchlist"
        } else {
            img.src = "/assets/icons/bookmark.svg"
            span.textContent = "Save"
            span2.textContent = "Add to watchlist"
        }
        msgContainer.replaceChildren(span,span2)


}

/** Performs a POST request to mark media with the argued id as watched. */
function initMarkAsWatchedButton(tmdbId,token) {
    const watchedButton = document.getElementById("mark-as-watched");
    const watchDate = watchedButton.dataset.watchDate
    updateMarkAsWatchedContents(watchDate)
    watchedButton.addEventListener("click", async ()=> {
        const response = await fetch(`?handler=MarkAsWatched&tmdbId=${tmdbId}`, {
            method: "POST",
            headers : {"RequestVerificationToken":token}
        });
        const data = await response.json()
        updateMarkAsWatchedContents(data.watchDate)

    })
    // todo: update button appearance.
}


function updateMarkAsWatchedContents(watchDate) {
    const span = document.createElement("span")
    const span2 = document.createElement("span")
    span2.className = "secondary-message";
    if (watchDate != "") {
        span.textContent = `Watched`
        span2.textContent = `at ${watchDate}`
    } else {
        span.textContent = "Mark as watched"
        span2.textContent ="add to history"
    }
    const msgContainer=document.getElementById("watched-button-msg")
    msgContainer.replaceChildren(span,span2);
}