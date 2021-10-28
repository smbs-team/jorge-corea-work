import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { dataURLtoFile, replaceDotBySymbol } from '../helpers/util';

export /**
 * Creates a Blob png from the root element on the DOM.
 * @return {Promise<Blob>} returns a Promise than will resolve on a Blob element.
 */
const createImageFromCurrentPage = async () => {
  return new Promise(resolve => {
    html2canvas(document.querySelector('#testDom')).then(canvas => {
      const imgData = canvas.toDataURL('image/png', 1);

      const pdf = new jsPDF('portrait');
      pdf.addImage(imgData, 'PNG', 10, 10, 150, 150);

      let document = pdf.output('blob');
      resolve(document);
      return document;
    });
  });
};
