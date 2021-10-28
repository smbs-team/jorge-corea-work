// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { Fragment } from 'react';
import CloseIcon from '@material-ui/icons/Close';
import ErrorIcon from '@material-ui/icons/Error';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import Loader from 'react-loader-spinner';

export interface ItemList {
  id: number | string;
  title: string;
  description: string;
  status?: string;
  date?: string;
  isJob?: boolean;
}

interface Props extends ItemList {
  handleDelete?: (id: string | number) => void;
}

const NotificationTimelineItem = (props: Props): JSX.Element => {
  const { id, title, description, date, status, isJob, handleDelete } = props;

  const onDelete = (): void => {
    handleDelete?.(id);
  };

  const getStatus = (): JSX.Element => {
    console.log(`props.status`, status);
    if (status) {
      if (status === 'Error') {
        return (
          <div className="NotificationTimelineItem-wrapper error">
            <ErrorIcon width={12} />{' '}
            <span className="NotificationTimelineItem-status">Job Failed</span>{' '}
            -<p className="NotificationTimelineItem-date">{date}</p>
          </div>
        );
      }
      if (status === 'Notification') {
        return (
          <div className="NotificationTimelineItem-wrapper success">
            <CheckCircleIcon width={12} />{' '}
            <span className="NotificationTimelineItem-status">
              Job Completed{' '}
            </span>{' '}
            -<p className="NotificationTimelineItem-date">{date}</p>
          </div>
        );
      }
      if (status === 'InProgress') {
        return (
          <div className="NotificationTimelineItem-wrapper success">
            <div className="NotificationTimelineItem-spinner">
              <Loader type="Oval" color="#00BFFF" height={12} width={12} />{' '}
            </div>
            <span className="NotificationTimelineItem-status">
              Job Running{' '}
            </span>{' '}
            -<p className="NotificationTimelineItem-date">{date}</p>
          </div>
        );
      }
    }
    return <Fragment />;
  };

  const renderCloseButton = (): JSX.Element => {
    if (!isJob) {
      return (
        <button onClick={onDelete} className="NotificationTimelineItem-dismiss">
          <CloseIcon fontSize="small" />
        </button>
      );
    }
    return <></>;
  };

  return (
    <div className="NotificationTimelineItem">
      {/* <p className="NotificationTimelineItem-date">22 AUG</p> */}
      <div className={`NotificationTimelineItem-content`}>
        <div className="NotificationTimelineItem-header">
          {getStatus()}
          {renderCloseButton()}
        </div>
        <p className="NotificationTimelineItem-title">{title}</p>
        <p className="NotificationTimelineItem-text">{description}</p>
      </div>
    </div>
  );
};

export default NotificationTimelineItem;
