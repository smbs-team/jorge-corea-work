import React from 'react';
import { arrayNullOrEmpty } from '../lib/helpers/util';
import { AuthContext } from './AuthContext';
import * as collectionCacheService from '../services/collectionCacheService';

import {
  getCountries,
  getCounties,
  getYears,
  getSeniorAppStatuses,
  getSeniorAppDetailStatuses,
  getRelationships,
  getAccountTypes,
  getExemptionSources,
  getExemptionTypes,
  getIncomeLevels,
  getMediaTypes,
  getPurposes,
  getSplitCodes,
  getDisabilityIncomeSources,
  getFinancialFilerTypes,
  getFinancialFormTypes,
  getMedicarePlans,
} from '../services/dynamics-service';

const CollectionContext = React.createContext();

class CollectionProvider extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      collectionsLoaded: false,
      loadingCollections: false,
      collectionCache: null,
      countries: [],
      counties: [],
      years: [],
      seniorAppStatuses: [],
      seniorAppDetailsStatuses: [],
      relationships: [],
      accountTypes: [],
      exemptionSources: [],
      exemptionTypes: [],
      incomeLevels: [],
      mediaTypes: [],
      purposes: [],
      splitCodes: [],
      disabilityIncomeSources: [],
      financialFilerTypes: [],
      financialFormTypes: [],
      medicarePlans: [],
      medicareOrganizations: [],
      portalAttachmentLocations: [],
      suffixes: [
        { locId: 'Suffix_', name: 'Select one', defaultLocString: null },
        { locId: 'Suffix_Jr', name: 'Jr.', defaultLocString: 'Jr.' },
        { locId: 'Suffix_Sr', name: 'Sr.', defaultLocString: 'Sr.' },
        { locId: 'Suffix_II', name: 'II', defaultLocString: 'II' },
        { locId: 'Suffix_III', name: 'III', defaultLocString: 'III' },
        { locId: 'Suffix_IV', name: 'IV', defaultLocString: 'IV' },
        { locId: 'Suffix_V', name: 'V', defaultLocString: 'V' },
        { locId: 'Suffix_VI', name: 'VI', defaultLocString: 'VI' },
      ],
      applicationStatus: [
        {
          id: 668020012,
          value: 'Unsubmitted',
        },
        {
          id: 668020013,
          value: 'New',
        },
        {
          id: 668020000,
          value: 'In Progress',
        },
        {
          id: 668020014,
          value: 'Pending - Taxpayer',
        },
        {
          id: 668020003,
          value: 'Pending - Archivist',
        },
        {
          id: 668020002,
          value: 'Pending - Appraiser',
        },
        {
          id: 668020003,
          value: 'Pending - Archivist',
        },
        {
          id: 668020001,
          value: 'Pending - Supervisor',
        },
        {
          id: 668020015,
          value: 'Escalated',
        },
      ],
    };
  }

  componentDidMount() {
    this.loadCollections();
  }

  componentDidUpdate() {
    if (!this.state.collectionsLoaded && !this.state.loadingCollections) {
      this.loadCollections();
    }
  }

  createTemplate = (name, id, defaultMessage) => {
    return `export const ${name} = (
      <FormattedMessage
        id="${id}"
        defaultMessage="${defaultMessage}"
      />
    ); `;
  };

  // Dev ONLY
  printLocalStrings = data => {
    let object = '';

    data.map(d => {
      object += this.createTemplate(d.locId, d.locId, d.defaultLocString);
    });

    console.log(object);
  };

  mapDataToLocalString = (data, key, addNoneOption = false) => {
    if (data) {
      data.map(d => {
        d.defaultLocString = d.value
          ? d.value.trim()
          : d.name
          ? d.name.trim()
          : '';

        d.locId = `${key}_${d.defaultLocString.replace(/\W/g, '_')}`;
        return d;
      });
    }

    // Hide log messages from prod environment
    if (process.env.NODE_ENV === 'development') {
      // this.printLocalStrings(data);
    }

    if (addNoneOption && data && data.length > 0) {
      let noneOption = {};
      // Create an empty copy of the base object to use for a filler none object value.
      // Then add the None properties.
      Object.keys(data[0]).forEach(prop => (noneOption[prop] = null));
      noneOption.defaultLocString = 'None';
      noneOption.locId = 'none';
      noneOption.name = null;
      noneOption.id = null;
      return [noneOption, ...data];
    }

    return data;
  };

  parceMedicarePlans = async collectionCache => {
    let medicarePlansData = await collectionCacheService.getCollection(
      'MedicarePlans',
      collectionCache,
      this.updateCacheCollectionState
    );
    if (!arrayNullOrEmpty(medicarePlansData)) {
      medicarePlansData = medicarePlansData.filter(m => m.approved);

      let parcedPlans = [medicarePlansData];
      let organizations = [];

      medicarePlansData.map(m => {
        let currentOrg = organizations.filter(
          o => o.name === m.organizationName
        );
        if (arrayNullOrEmpty(currentOrg)) {
          organizations.push({
            name: m.organizationName,
            id: m.id,
            approved: m.approved,
          });
        }
      });

      parcedPlans.push(organizations);
      return parcedPlans;
    }
  };

  setUpCountriesOrder = async collectionCache => {
    //defaultLocString

    let countries = await collectionCacheService.getCollection(
      'Countries',
      collectionCache,
      this.updateCacheCollectionState
    );
    if (!arrayNullOrEmpty(countries)) {
      let usa = countries.filter(c => c.name.trim() === 'United States')[0];
      let canada = countries.filter(c => c.name.trim() === 'Canada')[0];
      let mexico = countries.filter(c => c.name.trim() === 'Mexico')[0];

      let otherCountries = countries.filter(
        c =>
          c.name.trim() !== 'United States' &&
          c.name.trim() !== 'Canada' &&
          c.name.trim() !== 'Mexico'
      );

      countries = [usa, canada, mexico, ...otherCountries];
    }

    return countries;
  };

  updateCacheCollectionState = (collectionName, date) => {
    this.setState(
      prevState => {
        let cache = prevState.collectionCache;
        if (!cache || arrayNullOrEmpty(cache)) {
          cache = {};
        }
        cache[collectionName] = date;
        return { collectionCache: cache };
      },
      async () => {
        await collectionCacheService.setCacheInformation(
          this.state.collectionCache
        );
      }
    );
  };

  loadCollections = async () => {
    if (this.context.isLoggedIn()) {
      this.setState({ loadingCollections: true }, async () => {
        const collectionCache = await collectionCacheService.getCacheInformation();
        this.setState({ collectionCache });

        let promises = [
          collectionCacheService.getCollection(
            'Counties',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'Years',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'Relationships',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'AccountTypes',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'ExemptionSources',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'ExemptionTypes',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'IncomeLevels',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'MediaTypes',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'Purposes',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'SplitCodes',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'DisabilityIncomeSources',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'FinancialFilerTypes',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'FinancialFormTypes',
            collectionCache,
            this.updateCacheCollectionState
          ),
          this.setUpCountriesOrder(collectionCache),
          this.parceMedicarePlans(collectionCache),
          collectionCacheService.getCollection(
            'SeniorAppStatuses',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'SeniorAppDetailStatuses',
            collectionCache,
            this.updateCacheCollectionState
          ),
          collectionCacheService.getCollection(
            'PortalAttachmentLocations',
            collectionCache,
            this.updateCacheCollectionState
          ),
        ];
        let responses = await Promise.all(promises);
        let medicareInfo = responses[14];
        this.setState({
          collectionsLoaded: true,
          loadingCollections: false,
          counties: this.mapDataToLocalString(responses[0], 'counties'),
          years: this.mapDataToLocalString(responses[1], 'years'),
          relationships: this.mapDataToLocalString(
            responses[2],
            'OS_relationships'
          ),
          accountTypes: this.mapDataToLocalString(
            responses[3],
            'OS_accountTypes'
          ),
          exemptionSources: this.mapDataToLocalString(
            responses[4],
            'OS_exemptionSources'
          ),
          exemptionTypes: this.mapDataToLocalString(
            responses[5],
            'OS_exemptionTypes'
          ),
          incomeLevels: this.mapDataToLocalString(
            responses[6],
            'OS_incomeLevels'
          ),
          mediaTypes: this.mapDataToLocalString(responses[7], 'OS_mediaTypes'),
          purposes: this.mapDataToLocalString(responses[8], 'OS_purposes'),
          splitCodes: this.mapDataToLocalString(responses[9], 'OS_splitCodes'),
          disabilityIncomeSources: this.mapDataToLocalString(
            responses[10],
            'OS_disabilityIncomeSources'
          ),
          financialFilerTypes: this.mapDataToLocalString(
            responses[11],
            'OS_financialFilerTypes'
          ),
          financialFormTypes: this.mapDataToLocalString(
            responses[12],
            'OS_financialFormTypes'
          ),
          countries: this.mapDataToLocalString(responses[13], 'countries'),
          medicarePlans: this.mapDataToLocalString(
            arrayNullOrEmpty(medicareInfo) ? [] : medicareInfo[0],
            'medicarePlans',
            true
          ),
          medicareOrganizations: this.mapDataToLocalString(
            arrayNullOrEmpty(medicareInfo) ? [] : medicareInfo[1],
            'medicareOrganizations',
            true
          ),
          seniorAppStatuses: this.mapDataToLocalString(
            responses[15],
            'seniorAppStatuses'
          ),
          seniorAppDetailsStatuses: this.mapDataToLocalString(
            responses[16],
            'seniorAppDetailsStatuses'
          ),
          portalAttachmentLocations: this.mapDataToLocalString(
            responses[17],
            'OS_portalAttachmentLocations'
          ),
        });
      });
    }
  };

  render = () => {
    console.log('CollectionContext: ', this.state);
    return (
      <CollectionContext.Provider
        value={{
          collectionsLoaded: this.state.collectionsLoaded,
          suffixes: this.state.suffixes,
          countries: this.state.countries,
          counties: this.state.counties,
          years: this.state.years,
          relationships: this.state.relationships,
          accountTypes: this.state.accountTypes,
          exemptionSources: this.state.exemptionSources,
          exemptionTypes: this.state.exemptionTypes,
          incomeLevels: this.state.incomeLevels,
          mediaTypes: this.state.mediaTypes,
          purposes: this.state.purposes,
          splitCodes: this.state.splitCodes,
          disabilityIncomeSources: this.state.disabilityIncomeSources,
          financialFilerTypes: this.state.financialFilerTypes,
          financialFormTypes: this.state.financialFormTypes,
          medicarePlans: this.state.medicarePlans,
          medicareOrganizations: this.state.medicareOrganizations,
          applicationStatus: this.state.applicationStatus,
          seniorAppStatuses: this.state.seniorAppStatuses,
          seniorAppDetailsStatuses: this.state.seniorAppDetailsStatuses,
          portalAttachmentLocations: this.state.portalAttachmentLocations,
        }}
      >
        {this.props.children}
      </CollectionContext.Provider>
    );
  };
}

const CollectionConsumer = CollectionContext.Consumer;
CollectionProvider.contextType = AuthContext;
export { CollectionProvider, CollectionConsumer, CollectionContext };
