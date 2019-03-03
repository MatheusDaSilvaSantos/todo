namespace TodoList.UnitTests
{
    using System;
    using Xunit;
    using TodoList.Core;
    using TodoList.Core.UseCases.AddTask;
    using TodoList.Core.UseCases;
    using Moq;

    public sealed class TaskTests
    {
        [Fact]
        public void AddTask_ThrowsException_WhenNullInput()
        {
            IUseCase addTask = new Interactor(null);
            Assert.Throws<Exception>(() => addTask.Execute(null));
        }

        [Fact]
        public void AddTask_ThrowsException_WhenNullTitle()
        {
            Builder builder = new Builder();
            IUseCase addTask = new Interactor(null);
            Assert.Throws<Exception>(() => addTask.Execute(builder.Build()));
        }

        [Fact]
        public void AddTask_ThrowsException_WhenEmptyTitle()
        {
            Builder builder = new Builder();
            builder.WithTitle(string.Empty);
            IUseCase addTask = new Interactor(null);
            Assert.Throws<Exception>(() => addTask.Execute(builder.Build()));
        }

        [Fact]
        public void AddTask_InvokeOutputHandler_WhenNotNullTitle()
        {
            var outputHandler = new Mock<IOutputHandler>();
            Builder builder = new Builder();
            builder.WithTitle("My Title");
            IUseCase addTask = new Interactor(outputHandler.Object);
            addTask.Execute(builder.Build());
            outputHandler.Verify(e => e.Handle(It.IsAny<Output>()), Times.Once);
        }

        [Fact]
        public void AddTask_ReturnsId_WhenNotNullTitle()
        {
            Output actualOutput = null;
            var outputHandler = new Mock<IOutputHandler>();
            outputHandler.Setup(e => e.Handle(It.IsAny<Output>()))
                .Callback<Output>(output => actualOutput = output);

            Builder builder = new Builder();
            builder.WithTitle("My Title");
            IUseCase addTask = new Interactor(outputHandler.Object);
            addTask.Execute(builder.Build());
            Assert.True(actualOutput.Id != Guid.Empty);
        }
    }
}
