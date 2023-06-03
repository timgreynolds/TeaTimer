﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Represents an interface that can be used to provide platform-specific
    /// access to an underlying data provider. Type T is the type of the data
    /// object or model.
    /// </summary>
    public interface IDataService<T>
    {
        /// <summary>
        /// Provide initial setup of the data provider. If you need to setup
        /// connections to a database, for example, do it here.
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Retrieve the contents of the data provider.
        /// </summary>
        /// <returns>
        /// A list of type T objects.
        /// </returns>
        public List<T> Get();

        /// <summary>
        /// Retrieve the contents of the data provider in an asynchrous manner.
        /// </summary>
        /// <returns>
        /// A Task representing the retrieval operation. The Task result
        /// contains a list of type T objects.
        /// </returns>
        public Task<List<T>> GetAsync();

        /// <summary>
        /// Retrieve a specific object from the data provider by its unique
        /// identifier.
        /// </summary>
        /// <param name="id">
        /// The unigue identifier of the object to be retrieved.
        /// </param>
        /// <returns>
        /// The type T object represented by <paramref name="id">id</paramref>
        /// </returns>
        public T FindById(object id);

        /// <summary>
        /// Retrieve a specific object from the data provider by its unique
        /// identifier in an asynchrous manner.
        /// </summary>
        /// <param name="id">
        /// The unigue identifier of the object to be retrieved.
        /// </param>
        /// <returns>
        /// A Task representing the retrieval operation. The task result
        /// contains the type T object found.
        /// <paramref name="id">id</paramref>
        /// </returns>
        public Task<T> FindByIdAsync(object id);

        /// <summary>
        /// Add a new type T object to the data provider.
        /// </summary>
        /// <param name="obj">
        /// A type T object to be added to the database.
        /// </param>
        /// <returns>
        /// True if the object was added false if not.
        /// </returns>
        public T Add(object obj);

        /// <summary>
        /// Add a new type T object to the data provider in an asynchrous manner.
        /// </summary>
        /// <param name="obj">
        /// A type T object to be added to the database.
        /// </param>
        /// <returns>
        /// A Task representing the add operation. The task result contains the
        /// result of the operation.
        /// </returns>
        public Task<T> AddAsync(object obj);

        /// <summary>
        /// Update an existing object in the data provider.
        /// </summary>
        /// <param name="obj">
        /// A type T object to be added to the database.
        /// </param>
        /// <returns>
        /// The type T object as updated.
        /// </returns>
        public T Update(object obj);

        /// <summary>
        /// Update an existing object in the data provider in an asynchrous
        /// manner.
        /// </summary>
        /// <param name="obj">
        /// A type T object to be added to the database.
        /// </param>
        /// <returns>
        /// A Task representing the update operation. The task result contains
        /// the type T object as updated.
        /// </returns>
        public Task<T> UpdateAsync(object obj);

        /// <summary>
        /// Delete an existing object from the data provider.
        /// </summary>
        /// <param name="obj">
        /// The type T object to be deleted.
        /// </param>
        /// <returns>
        /// True if the object was deleted false if not.
        /// </returns>
        public bool Delete(object obj);

        /// <summary>
        /// Delete an existing object from the data provider in an asynchronous
        /// manner.
        /// </summary>
        /// <param name="obj">
        /// The type T object to be deleted.
        /// </param>
        /// <returns>
        /// A Task representing the delete operation. The task result contais
        /// the result of the operation.
        /// </returns>
        public Task<bool> DeleteAsync(object obj);
    }
}