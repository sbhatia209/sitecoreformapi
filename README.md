# 💡 Sitecore Headless Form Integration (Angular POC)

This repository demonstrates a **proof of concept (POC)** for integrating **Sitecore Forms** with **headless frontend frameworks** like Angular, while maintaining **Sitecore's out-of-the-box (OOTB) functionality** for storing form submissions.

---

## 🧠 Background

A common challenge when using **Sitecore with headless setups** is the lack of documentation around **Sitecore Forms** integration—especially when using frontend frameworks like **Angular**.

To address this, this project outlines an approach where:
- Sitecore forms are created and rendered using the **Forms Extension module**.
- An API endpoint is created in a separate project to receive and save submitted form data (including file uploads) directly into the **Sitecore Forms database**.

---

## 🛠️ Key Features

- ✅ Sitecore OOTB form rendering via Layout Service
- ✅ Custom Web API to receive form data and store it in Sitecore
- ✅ File upload handling using Sitecore's `FileStorageProvider`
- ✅ Preserves native Sitecore Forms functionality, reporting, and export

---

## 🚀 Setup Guide

### 1. Create Sitecore Form Using Form Extension

Use the **Form Extension** to design your form in Sitecore. Add the OOTB form rendering to your page to make form fields available via the **Layout Service**.

### 2. Create API Endpoint to Save Form Submissions

