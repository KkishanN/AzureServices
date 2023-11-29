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
        bidtext.innerHTML = newBid;
        input.value = newBid + 1;
    });

    connection.start().catch(err => console.error(err.toString()));
    return connection;
}

const connection = initializeSignalRConnection();

// connection is established


const submitBid = (auctionId) => {
    const bid = document.getElementById(auctionId + "-input").value;
    //const url = `/auction/${auctionId}/newbid?currentBid=${bid}`;
    //fetch(url, {
    //    method: "POST",
    //    headers: {
    //        'Content-Type': 'application/json'
    //    }
    //});

    // call methods using signalR
    connection.invoke("NotifyNewBid", {
        auctionId: parseInt(auctionId),
        newBid: parseInt(bid)
    });
}
