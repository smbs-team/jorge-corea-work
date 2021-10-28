require("dotenv").config();
const express = require("express");
const app = express();
const cors = require("cors");
const bodyParser = require("body-parser");
const multer = require("multer");
const { Storage } = require("@google-cloud/storage");
const fileUploadCredentials = require("./fileUploadCredentials.json");
const testFiles = require("./files.json");
const port = process.env.API_PORT || 8080;

//Most of this code was taken from: https://betterprogramming.pub/how-to-upload-files-to-firebase-cloud-storage-with-react-and-node-js-e87d80aeded1

//#region Firebase storage
const storageSettings = {
  GCLOUD_PROJECT_ID: fileUploadCredentials.project_id,
  GCLOUD_APPLICATION_CREDENTIALS:
    /*__dirname +*/ "api/fileUploadCredentials.json",
  GCLOUD_STORAGE_BUCKET_URL: fileUploadCredentials.project_id + ".appspot.com"
};
const storage = new Storage({
  projectId: storageSettings.GCLOUD_PROJECT_ID,
  keyFilename: storageSettings.GCLOUD_APPLICATION_CREDENTIALS
});
const bucket = storage.bucket(storageSettings.GCLOUD_STORAGE_BUCKET_URL);
//#endregion

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cors());
app.options("*", cors());
app.get("/", (req, res) => res.send("File upload API ready for use"));

// Initiating a memory storage engine to store files as Buffer objects
const uploader = multer({
  storage: multer.memoryStorage(),
  limits: {
    fileSize: 5 * 1024 * 1024 // keep images size < 5 MB
  }
});

// Upload endpoint to send file to Firebase storage bucket
// app.options("/api/upload", cors()); // enable pre-flight request for post request
app.post(
  "/api/realUpload",
  uploader.single("image"),
  async (req, res, next) => {
    try {
      console.log("req.file:", req.file);
      if (!req.file) {
        res.status(400).send("No file uploaded.");
        return;
      }
      // This is where we'll upload our file to Cloud Storage
      const blob = bucket.file(req.file.originalname);
      const blobStream = blob.createWriteStream({
        metadata: {
          contentType: req.file.mimetype
        }
      });

      // If there's an error
      blobStream.on("error", (err) => next(err));
      // If all is good and done
      blobStream.on("finish", () => {
        // Assemble the file public URL
        const publicUrl = `https://firebasestorage.googleapis.com/v0/b/${
          bucket.name
        }/o/${encodeURI(blob.name)}?alt=media`;
        // Return the file name and its public URL
        // for you to store in your own database
        res.status(200).send({
          fileName: req.file.originalname,
          fileLocation: publicUrl
        });
      });
      // When there is no more data to be consumed from the stream the end event gets emitted
      blobStream.end(req.file.buffer);
    } catch (error) {
      res.status(400).send(`Error, could not upload file: ${error}`);
      return;
    }
  }
);

app.post("/api/upload", uploader.single("image"), async (req, res, next) => {
  try {
    console.log("req.file:", req.file);
    if (!req.file) {
      res.status(400).send("No file uploaded.");
      return;
    }

    //TEMP: send test file
    setTimeout(() => {
      res.status(200).send({
        fileName: req.file.originalname,
        content: testFiles.file3
      });
    }, 3000);
  } catch (error) {
    res.status(400).send(`Error, could not upload file: ${error}`);
    return;
  }
});

app.post("/api/download", async (req, res, next) => {
  try {
    //TEMP: send test files
    res.status(200).send({
      files: [
        {
          fileName: "file1.png",
          content: testFiles.file1
        },
        {
          fileName: "file2.jpg",
          content: testFiles.file2
        }
      ]
    });
  } catch (error) {
    res.status(400).send(`Error, could not download file: ${error}`);
    return;
  }
});

app.listen(port, () =>
  console.log(`File uploader API listening on port ${port}`)
);
