import { useState, useRef, useEffect } from "react";
import axios from "axios";
import {
  Container,
  Paper,
  Typography,
  TextField,
  Button,
  Box
} from "@mui/material";

function Chat() {
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState([]);

  const messagesEndRef = useRef(null);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({
      behavior: "smooth"
    });
  }, [messages]);

  const sendMessage = async () => {
    if (!message.trim()) return;

    const userMessage = {
      role: "user",
      text: message
    };

    setMessages((prev) => [...prev, userMessage]);

    const currentMessage = message;
    setMessage("");

    try {
      const result = await axios.post(
        "https://packagemanagement-api-hcevhrcwhmbsa6d7.centralus-01.azurewebsites.net/api/chat",
        {
          message: currentMessage
        }
      );

      const assistantMessage = {
        role: "assistant",
        text: result.data
      };

      setMessages((prev) => [...prev, assistantMessage]);
    } catch (error) {
      setMessages((prev) => [
        ...prev,
        {
          role: "assistant",
          text: "Unable to connect to Package Management API."
        }
      ]);
    }
  };

  return (
    <Container maxWidth="lg" sx={{ mt: 3 }}>
      <Paper elevation={4} sx={{ p: 3 }}>
        <Typography
          variant="h2"
          align="center"
          gutterBottom
        >
          Package Management Assistant
        </Typography>

        {/* Chat Area */}
        <Paper
          elevation={1}
          sx={{
            height: "600px",
            overflowY: "auto",
            p: 2,
            backgroundColor: "#f5f5f5",
            mb: 2
          }}
        >
          {messages.length === 0 && (
            <Typography
              align="center"
              color="text.secondary"
              sx={{ mt: 20 }}
            >
              Start chatting with Package Management Assistant...
            </Typography>
          )}

          {messages.map((msg, index) => (
            <Box
              key={index}
              sx={{
                display: "flex",
                justifyContent:
                  msg.role === "user"
                    ? "flex-end"
                    : "flex-start",
                mb: 2
              }}
            >
              <Paper
                elevation={3}
                sx={{
                  p: 2,
                  maxWidth: "75%",
                  backgroundColor:
                    msg.role === "user"
                      ? "#1976d2"
                      : "#ffffff",
                  color:
                    msg.role === "user"
                      ? "white"
                      : "black"
                }}
              >
                <Typography
                  variant="subtitle2"
                  sx={{
                    fontWeight: "bold",
                    mb: 1
                  }}
                >
                  {msg.role === "user"
                    ? "You"
                    : "Assistant"}
                </Typography>

                <Typography
                  sx={{
                    whiteSpace: "pre-wrap"
                  }}
                >
                  {msg.text}
                </Typography>
              </Paper>
            </Box>
          ))}

          <div ref={messagesEndRef}></div>
        </Paper>

        {/* Input Area */}
        <Box
          sx={{
            display: "flex",
            gap: 2
          }}
        >
          <TextField
            fullWidth
            placeholder="Ask something..."
            value={message}
            onChange={(e) =>
              setMessage(e.target.value)
            }
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                sendMessage();
              }
            }}
          />

          <Button
            variant="contained"
            size="large"
            onClick={sendMessage}
            sx={{
              minWidth: "120px"
            }}
          >
            Send
          </Button>
        </Box>
      </Paper>
    </Container>
  );
}

export default Chat;