// useGetNbhd.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import MajorOffice from 'components/home/areas/commercial/majorOffice';
import Hotels from 'components/home/areas/hotels';
import CommercialIncome from 'components/home/areas/commercial/income';
import useAreaTreeStore from 'stores/useAreaTreeStore';
import FloatingHome from 'components/home/areas/floatingHome';
import NotFound from 'components/NotFound';

export type Area = 'hotels' | 'majorOffice' | 'floatingHome' | 'commercial';

interface ToReturn {
  NbhdComponent: JSX.Element;
  area?: string;
  nbhdNumber?: string;
  name?: Area;
}

function useGetArea(): ToReturn {
  const areaTreeStore = useAreaTreeStore();
  const item = areaTreeStore.selectedItem;
  const area = item?.area?.entityName;
  const nbhdNumber = item?.area?.ptasName;

  if (!area || !nbhdNumber)
    return {
      NbhdComponent: <NotFound />,
      area: undefined,
      nbhdNumber: undefined,
    };

  if (area === 'PtasSpecialtyarea') {
    switch (nbhdNumber) {
      case '160':
        return { NbhdComponent: <Hotels />, area, nbhdNumber, name: 'hotels' };
      case '280':
        return {
          NbhdComponent: <MajorOffice />,
          area,
          nbhdNumber,
          name: 'majorOffice',
        };
      case '015':
        return {
          NbhdComponent: <FloatingHome />,
          area,
          nbhdNumber,
          name: 'floatingHome',
        };
      default:
        return {
          NbhdComponent: <CommercialIncome />,
          area,
          nbhdNumber,
          name: 'commercial',
        };
    }
  } else {
    switch (nbhdNumber) {
      default:
        return {
          NbhdComponent: <CommercialIncome />,
          area,
          nbhdNumber,
          name: 'commercial',
        };
    }
  }
}

export default useGetArea;
