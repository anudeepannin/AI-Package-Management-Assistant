import { useState } from "react";
import axios from "axios";

function ChatPage() {
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState([]);

  const sendMessage = async () => {
    if (!message.trim()) return;

    const userMessage = {
      role: "user",
      content: message,
    };

    setMessages((prev) => [...prev, userMessage]);

    try {
      const response = await axios.post(
        "https://localhost:7091/api/chat",
        {
          message: message,
        }
      );

      const assistantMessage = {
        role: "assistant",
        content: response.data,
      };

      setMessages((prev) => [
        ...prev,
        assistantMessage,
      ]);

      setMessage("");
    } catch (error) {
      console.error(error);

      const errorMessage = {
        role: "assistant",
        content: "Error calling API",
      };

      setMessages((prev) => [
        ...prev,
        errorMessage,
      ]);
    }
  };

  const clearChat = () => {
    setMessages([]);
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      sendMessage();
    }
  };

  return (
    <div
      style={{
        maxWidth: "1000px",
        margin: "0 auto",
        padding: "20px",
        fontFamily: "Arial",
      }}
    >
      <h1>AI Package Management Assistant</h1>

      <button
        onClick={clearChat}
        style={{
          marginBottom: "10px",
          padding: "8px 12px",
        }}
      >
        Clear Chat
      </button>

      <div
        style={{
          height: "500px",
          overflowY: "auto",
          border: "1px solid #ddd",
          padding: "15px",
          marginBottom: "15px",
          borderRadius: "8px",
          backgroundColor: "#f9f9f9",
        }}
      >
        {messages.length === 0 && (
          <p>Start a conversation...</p>
        )}

        {messages.map((msg, index) => (
          <div
            key={index}
            style={{
              marginBottom: "15px",
              textAlign:
                msg.role === "user"
                  ? "right"
                  : "left",
            }}
          >
            <div
              style={{
                display: "inline-block",
                padding: "10px",
                borderRadius: "10px",
                maxWidth: "75%",
                backgroundColor:
                  msg.role === "user"
                    ? "#d1e7dd"
                    : "#ffffff",
                border: "1px solid #ccc",
                whiteSpace: "pre-wrap",
              }}
            >
              <strong>
                {msg.role === "user"
                  ? "You"
                  : "Assistant"}
              </strong>

              <br />

              {msg.content}
            </div>
          </div>
        ))}
      </div>

      <textarea
        rows="4"
        value={message}
        onChange={(e) =>
          setMessage(e.target.value)
        }
        onKeyDown={handleKeyDown}
        placeholder="Ask about package status, renewals, support policies..."
        style={{
          width: "100%",
          padding: "10px",
          borderRadius: "8px",
        }}
      />

      <br />
      <br />

      <button
        onClick={sendMessage}
        style={{
          padding: "10px 20px",
          cursor: "pointer",
        }}
      >
        Send
      </button>
    </div>
  );
}

export default ChatPage;