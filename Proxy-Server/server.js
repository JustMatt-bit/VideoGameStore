const express = require('express');
const fetch = require('node-fetch');
const app = express();

app.use(express.json()); // For parsing application/json

const FEDEX_API_URL = 'https://apis-sandbox.fedex.com/rate/v1/rates/quotes';

app.post('/proxy/rate/v1/rates/quotes', async (req, res) => {
    try {
        const response = await fetch(FEDEX_API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Add your FedEx API Key and other necessary headers here
                'Authorization': 'Bearer l702a5cad1fbc94f9c8e2e9c35f6706167'
            },
            body: JSON.stringify(req.body)
        });

        if (!response.ok) {
            throw new Error(`FedEx API responded with status: ${response.status}`);
        }

        const data = await response.json();
        res.json(data);
    } catch (error) {
        res.status(500).send(error.message);
    }
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
