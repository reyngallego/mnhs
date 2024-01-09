class WebSocketManager {
    constructor(url) {
        this.url = url;
        this.socket = null;
    }

    connect() {
        this.socket = new WebSocket(this.url);

        this.socket.onopen = (event) => {
            console.log('WebSocket connection opened:', event);
        };

        this.socket.onmessage = (event) => {
            console.log('WebSocket message received:', event);
            // You can process the received message here
        };

        this.socket.onclose = (event) => {
            console.log('WebSocket connection closed:', event);
        };

        this.socket.onerror = (event) => {
            console.error('WebSocket error:', event);
        };
    }

    send(message) {
        if (this.socket && this.socket.readyState === WebSocket.OPEN) {
            this.socket.send(message);
        } else {
            console.error('WebSocket is not open for sending.');
        }
    }

    close() {
        if (this.socket) {
            this.socket.close();
        }
    }
}

// Example usage:
const webSocketManager = new WebSocketManager('wss://localhost:65388');
webSocketManager.connect();

// To send a message:
webSocketManager.send('Hello, WebSocket!');

// To close the connection:
// webSocketManager.close();
