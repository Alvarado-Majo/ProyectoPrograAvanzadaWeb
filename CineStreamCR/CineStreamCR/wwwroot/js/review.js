// ════════════════════════════════════════════════════
// REVIEWS ESTILO NETFLIX (manita arriba / manita abajo)
// ════════════════════════════════════════════════════

function loadReviewSummary(movieId) {

    fetch(`/Review/Summary?movieId=${movieId}`)
        .then(response => response.json())
        .then(result => {

            if (!result.esCorrecto) {
                return;
            }

            renderReviewSummary(result.dato, result.userLoggedIn, result.userVoteIsLike);
        })
        .catch(() => {
            // Falla silenciosa: el bloque de rating simplemente no se actualiza
        });
}

function renderReviewSummary(summary, userLoggedIn, userVoteIsLike) {

    const ratingEl = document.getElementById("reviewRatingValue");
    const countEl = document.getElementById("reviewCount");
    const likeBtn = document.getElementById("reviewLikeBtn");
    const dislikeBtn = document.getElementById("reviewDislikeBtn");

    if (!ratingEl || !countEl || !likeBtn || !dislikeBtn) {
        return;
    }

    // Sin reviews todavía: se muestra 10 por defecto (regla del negocio)
    const displayRating = summary.movieRating != null
        ? summary.movieRating.toFixed(1)
        : "10.0";

    ratingEl.textContent = displayRating;
    countEl.textContent = summary.totalReviews === 1
        ? "1 review"
        : `${summary.totalReviews} reviews`;

    likeBtn.classList.remove("active");
    dislikeBtn.classList.remove("active");

    if (userLoggedIn && userVoteIsLike === true) {
        likeBtn.classList.add("active");
    } else if (userLoggedIn && userVoteIsLike === false) {
        dislikeBtn.classList.add("active");
    }
}

function voteReview(movieId, isLike) {

    const token = document.querySelector(
        'input[name="__RequestVerificationToken"]'
    )?.value;

    fetch("/Review/Vote", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body: new URLSearchParams({
            movieId: movieId,
            isLike: isLike,
            __RequestVerificationToken: token ?? ""
        })
    })
        .then(response => response.json())
        .then(result => {

            if (result.requiresLogin) {
                window.location.href = "/Auth/Login";
                return;
            }

            if (!result.esCorrecto) {
                alert(result.mensaje ?? "Could not register your vote.");
                return;
            }

            loadReviewSummary(movieId);
        })
        .catch(() => {
            alert("Could not register your vote. Please try again.");
        });
}

document.addEventListener("DOMContentLoaded", function () {

    const widget = document.getElementById("reviewWidget");

    if (!widget) {
        return;
    }

    const movieId = widget.dataset.movieId;

    loadReviewSummary(movieId);

    document.getElementById("reviewLikeBtn")
        .addEventListener("click", () => voteReview(movieId, true));

    document.getElementById("reviewDislikeBtn")
        .addEventListener("click", () => voteReview(movieId, false));
});
