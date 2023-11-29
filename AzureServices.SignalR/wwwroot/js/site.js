// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const { signalR } = require("./signalr/dist/browser/signalr");

const clickme = () => {
    alert("Go to heaven!!!!!!!!!");
}

// create connection to the signalR
const initializeSignalRConnection = () => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/auctionhub")
        .build();

    //subscribe
    connection.on("ReceiveNewBid", ({ auctionId, newBid }) => {
        const tr = document.getElementById(auctionId + "-tr");
        const input = document.getElementById(auctionId + "-input");

        // start animation
        tr.classList.add("animate-highlight");
        setTimeout(() => tr.classList.remove("animate-highligh"), 2000);

        const bidtext = document.getElementById(auctionId + "-bidText");
        input.value = newBid + 1;
    });

    connection.start().catch(err => console.error(err.toString()));
    return connection;
}

// connection is established
const connection = initializeSignalRConnection();

const submitBid = (auctionId) => {
    const bid = document.getElementById(auctionId + "-input").value;
    fetch("/auction/" + auctionId + "/newbid?currentBid=" + bid, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        }
    });
    // location.reload();

    // call methods using signalR
    connection.invoke("NotifyNewBid", {
        auctionId: parseInt(auctionId),
        newBid: parseInt(bid)
    });
}
