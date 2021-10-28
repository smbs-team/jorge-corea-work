import React from 'react';
import renderer from 'react-test-renderer';
import UploadFile from './UploadFile';
import {
  render,
  fireEvent,
  cleanup,
  waitForElement,
  wait,
} from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import {
  isValidImage,
  cancelRequests,
} from '../../../services/cognitiveService';
import {
  uploadFile,
  deleteBlob,
  getImage,
} from '../../../services/blobService';
import { createUrl } from '../../common/UploadFile/helpers';

jest.mock('../../../services/cognitiveService', () => ({
  isValidImage: jest.fn(),
  cancelRequests: jest.fn(),
}));

jest.mock('../../common/UploadFile/helpers', () => ({
  createUrl: jest.fn(),
}));

jest.mock('../../../services/blobService', () => ({
  uploadFile: jest.fn(),
  deleteBlob: jest.fn(),
  getImage: jest.fn(),
}));

let images;
beforeEach(() => {
  images = [createFile('cats.jpg', 127, 'image/png')];
});

afterEach(() => {
  cleanup();
  jest.clearAllMocks();
});

const ui = (
  <UploadFile
    leftMessage={{ id: 'my_id', defaultMessage: 'Left message' }}
    rightMessage={{ id: 'my_id2', defaultMessage: 'Right message' }}
    obscureInfoMessage={false}
    fileArray={[]}
    onArrayUpdate={() => {}}
    appId={'2929929'}
    detailsId={'5993949'}
  />
);

//snapshot test
test('renders correctly', () => {
  const tree = renderer
    .create(
      <UploadFile
        leftMessage={{ id: 'my_id', defaultMessage: 'Left message' }}
        rightMessage={{ id: 'my_id2', defaultMessage: 'Right message' }}
        obscureInfoMessage={false}
        fileArray={[]}
        onArrayUpdate={() => {}}
      />
    )
    .toJSON();
  expect(tree).toMatchSnapshot();
});

test('show the selected images are loading', async () => {
  const { container, getByText } = render(ui);

  const files = [
    createFile('cats.jpg', 127, 'image/png'),
    createFile('dog.jpg', 127, 'image/png'),
  ];

  await sendFiles(container, files);

  expect(getByText('cats.jpg')).toBeVisible();
  expect(getByText('dog.jpg')).toBeVisible();
});

test('show the received images from outside', async () => {
  const ui2 = (
    <UploadFile
      leftMessage={{ id: 'my_id', defaultMessage: 'Left message' }}
      rightMessage={{ id: 'my_id2', defaultMessage: 'Right message' }}
      obscureInfoMessage={false}
      fileArray={[
        {
          imageName: 'document.jpg',
          isLoading: false,
          isValid: false,
          isUploading: false,
        },
      ]}
      onArrayUpdate={() => {}}
    />
  );

  const { getByText } = render(ui2);

  expect(getByText('document.jpg')).toBeVisible();
});

test('process the image and show redaction tool', async () => {
  const { container, getByTestId } = render(ui);

  await sendFiles(container, images);

  createUrl.mockReturnValue('abc');
  isValidImage.mockReturnValue(false);

  const imageCrossOut = await waitForElement(() => getByTestId('continue'));

  expect(imageCrossOut).toBeInTheDocument();
});

test('error message is shown when invalid file is passed', async () => {
  const { container, getByTestId } = render(ui);

  await sendFiles(container, [createFile('cats.jpg', 1, 'image/ppp')]);

  expect(getByTestId('generalError')).toHaveTextContent(
    'The document type you are trying to attach is invalid. Please try one of the valid document types (.pdf, .jpg, .jpeg, .png, .gif)'
  );
});

test('error message is shown when the image is bigger than 4MB', async () => {
  const { container, getByTestId } = render(ui);

  await sendFiles(container, [createFile('cats.jpg', 4194604, 'image/gif')]);

  expect(getByTestId('specificError')).toHaveTextContent(
    'The document you are trying to attach is over the allowable file size of 4 MB.'
  );
});

test('show image is not valid message', async () => {
  const { container, getByTestId } = render(ui);

  await sendFiles(container, images);

  createUrl.mockReturnValue('abc');
  isValidImage.mockReturnValue(false);

  const error = await waitForElement(() => getByTestId('specificError'));

  expect(error).toHaveTextContent(
    "We can't upload your document because we can't find any readable text."
  );

  expect(isValidImage).toHaveBeenCalled();
  expect(createUrl).toHaveBeenCalled();
});

test('remove item from the DOM when cancelling the request via the cancel button while loading', async () => {
  const { container, getByTestId, queryByText } = render(ui);

  await sendFiles(container, images);

  fireEvent.click(getByTestId('deleteIconLoading'));

  expect(cancelRequests).toHaveBeenCalled();
  expect(queryByText('cats.jpg')).not.toBeInTheDocument();
});

test('remove invalid items when adding a new one', async () => {
  const { container, queryByText, getByTestId } = render(ui);

  await sendFiles(container, images);

  createUrl.mockReturnValue('abc');
  isValidImage.mockReturnValue(false);

  const error = await waitForElement(() => getByTestId('specificError'));

  expect(error).toHaveTextContent(
    "We can't upload your document because we can't find any readable text."
  );

  await sendFiles(container, [createFile('gorilla.jpg', 127, 'image/png')]);

  expect(queryByText('cats.jpg')).not.toBeInTheDocument();
  expect(queryByText('gorilla.jpg')).toBeInTheDocument();
});

test('throw error when trying to add an already processed image', async () => {
  const { container, getByTestId } = render(ui);

  await sendFiles(container, images);

  createUrl.mockReturnValue('abc');
  isValidImage.mockReturnValue(true);

  await wait();

  await sendFiles(container, [createFile('cats.jpg', 127, 'image/png')]);

  const error = await waitForElement(() => getByTestId('generalError'));

  expect(error).toHaveTextContent('File already processed.');
});

test('upload is called when clicking continue', async () => {
  const { container, getByTestId } = render(ui);
  await sendFiles(container, images);

  createUrl.mockReturnValue('abc');
  isValidImage.mockReturnValue(true);

  const imageCrossOut = await waitForElement(() => getByTestId('continue'));

  expect(imageCrossOut).toBeInTheDocument();
  fireEvent.click(imageCrossOut);
  uploadFile();
  await wait();
  getImage().mockReturnValue('abc');
  await wait();
  createUrl.mockReturnValue('abc');
  expect(uploadFile).toHaveBeenCalled();
});

test('remove item from the DOM when deleting the item via the delete button when the image is uploaded', async () => {
  const { container, getByTestId, queryByText } = render(ui);
  await sendFiles(container, images);

  createUrl.mockReturnValue('abc');
  isValidImage.mockReturnValue(true);

  const imageCrossOut = await waitForElement(() => getByTestId('continue'));

  fireEvent.click(imageCrossOut);
  uploadFile();
  await wait();
  createUrl.mockReturnValue('abc');
  await wait();
  expect(getByTestId('deleteIconUploaded')).toBeEnabled();
  fireEvent.click(getByTestId('deleteIconUploaded'));

  expect(deleteBlob).toHaveBeenCalled();
  await wait();
  expect(queryByText('cats.jpg')).not.toBeInTheDocument();
});

test('open redaction tool when the zoom icon is clicked', async () => {
  const { container, getByTestId } = render(ui);

  await sendFiles(container, images);

  createUrl.mockReturnValue('abc');
  isValidImage.mockReturnValue(true);
  await wait();

  const imageCrossOut = await waitForElement(() => getByTestId('continue'));

  fireEvent.click(imageCrossOut);
  uploadFile();
  await wait();
  getImage().mockReturnValue('abc');
  await wait();
  createUrl.mockReturnValue('abc');
  expect(getByTestId('zoomIcon')).toBeEnabled();
  fireEvent.click(getByTestId('zoomIcon'));

  const continueButton = await waitForElement(() => getByTestId('continue'));

  expect(continueButton).toBeInTheDocument();
});

async function sendFiles(container, imagesArray) {
  const input = container.querySelector('input');

  Object.defineProperty(input, 'files', {
    value: imagesArray,
    configurable: true,
  });

  dispatchEvt(input, 'change');
  await flushPromises(ui, container);
}

function flushPromises(ui, container) {
  return new Promise(resolve =>
    setImmediate(() => {
      render(ui, { container });
      resolve(container);
    })
  );
}

function dispatchEvt(node, type, data) {
  const event = new Event(type, { bubbles: true });
  Object.assign(event, data);
  fireEvent(node, event);
}

function createFile(name, size, type) {
  const file = new File([], name, { type });
  Object.defineProperty(file, 'size', {
    get() {
      return size;
    },
  });
  return file;
}
