// Instruction.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomCard } from '@ptas/react-public-ui-library';
import { IconButton } from '@material-ui/core';
import CloseIcon from '@material-ui/icons/Close';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { useHistory } from 'react-router-dom';
import * as fm from './formatText';
import useStyles from './styles';

function Instruction(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();

  const handleClick = (): void => {
    history.goBack();
  };

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
          wrapperContent: classes.contentWrap,
        }}
      >
        <IconButton className={classes.closeButton} onClick={handleClick}>
          <CloseIcon className={classes.closeIcon} />
        </IconButton>

        <h2 className={classes.title}>{fm.mainTitle}</h2>
        <span className={classes.border}></span>
        {/* business */}
        <div className={classes.instructionWrapper}>
          <span className={classes.listTitle}>{fm.business}</span>
          <div className={classes.instructionWrapper}>
            <span className={classes.listSubtitle}>{fm.AddABusinessTitle}</span>
            <ul className={classes.mainList}>
              <li className={classes.mainItem}>
                {fm.addExisting}
                <ul className={classes.subList}>
                  <li className={classes.subItem}>{fm.addExistingDesc}</li>
                </ul>
              </li>
              <li className={classes.mainItem}>
                {fm.addANewUnlistedBusiness}
                <ul className={classes.subList}>
                  <li className={classes.subItem}>
                    {fm.addANewUnlistedBusinessDesc}
                  </li>
                </ul>
              </li>
              <li className={classes.mainItem}>
                {fm.reportYourBusiness}
                <ul className={classes.subList}>
                  <li className={classes.subItem}>
                    {fm.reportYourBusinessDesc}
                  </li>
                </ul>
              </li>
              <li className={classes.mainItem}>
                {fm.removeABusiness}
                <ul className={classes.subList}>
                  <li className={classes.subItem}>{fm.removeABusinessDesc}</li>
                </ul>
              </li>
            </ul>
          </div>
          <div className={classes.instructionWrapper}>
            <span className={classes.listSubtitle}>
              {fm.updatingBusinessTitle}
            </span>
            <span className={classes.description}>
              {fm.updatingBusinessTitleDesc}
            </span>
            <ul className={classes.mainList}>
              <li className={classes.mainItem}>
                {fm.updateViaOnline}
                <ul className={classes.subList}>
                  <li className={classes.subItem}>
                    {fm.updateViaOnlineInstructionItem1}
                  </li>
                  <li className={classes.subItem}>
                    {fm.updateViaOnlineInstructionItem2}
                  </li>
                  <li className={classes.subItem}>
                    {fm.updateViaOnlineInstructionItem3}
                  </li>
                </ul>
              </li>
              <li className={classes.mainItem}>
                {fm.updateViaAttached}
                <ul className={classes.subList}>
                  <li className={classes.subItem}>
                    {fm.updateViaAttachedListItem1}
                  </li>
                  <li className={classes.subItem}>
                    {fm.updateViaAttachedListItem2}
                  </li>
                </ul>
              </li>
            </ul>
          </div>
        </div>
        <div className={classes.instructionWrapper}>
          <span className={classes.listTitle}>{fm.signingIn}</span>
          <span className={classes.description}>{fm.signingInDes}</span>
          <ul className={classes.mainList}>
            <li className={classes.mainItem}>
              {fm.signWithEmail}
              <ul className={classes.subList}>
                <li className={classes.subItem}>{fm.signWithEmailListItem1}</li>
                <li className={classes.subItem}>{fm.signWithEmailListItem2}</li>
              </ul>
            </li>
            <li className={classes.mainItem}>
              {fm.signWithAnAccountProvider}
              <ul className={classes.subList}>
                <li className={classes.subItem}>
                  {fm.signWithAnAccountProviderListItem1}
                </li>
                <li className={classes.subItem}>
                  {fm.signWithAnAccountProviderListItem2}
                </li>
                <li className={classes.subItem}>
                  {fm.signWithAnAccountProviderListItem3}
                </li>
              </ul>
            </li>
            <span className={classes.description}>
              {fm.signingInInstruction}
            </span>
          </ul>
        </div>
        <div className={classes.instructionWrapper}>
          <span className={classes.listTitle}>{fm.people}</span>
          <span className={classes.description}>{fm.peopleDesc}</span>
          <ul className={classes.mainList}>
            <li className={classes.mainItem}>
              {fm.addAPerson}
              <ul className={classes.subList}>
                <li className={classes.subItem}>{fm.addAPersonListItem1}</li>
              </ul>
            </li>
            <li className={classes.mainItem}>
              {fm.editAPerson}
              <ul className={classes.subList}>
                <li className={classes.subItem}>{fm.editAPersonListItem1}</li>
                <li className={classes.subItem}>{fm.editAPersonListItem2}</li>
              </ul>
            </li>
          </ul>
        </div>
      </CustomCard>
    </MainLayout>
  );
}

export default Instruction;
